using System;
using System.Collections.Generic;
using hgame1.Graphics.Textures;
using OpenTK;
using hgame1.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.Models
{
	public class Mesh : IDisposable
	{
		public MeshElement[] Data;
		public uint[] Indices;

		public BeginMode MeshType;
		public BufferUsageHint DataBufferUsageHint;
		public BufferUsageHint IndexBufferUsageHint;

		protected int Vao, Vbo, Ebo;

		/// <summary>
		/// Initializes a new instance of the <see cref="MGLALLib.Graphics.Models.Mesh"/> class with default parameters.
		/// </summary>
		public Mesh () : this(BeginMode.Triangles, BufferUsageHint.StaticDraw, BufferUsageHint.StaticDraw){}

		/// <summary>
		/// Initializes a new instance of the <see cref="MGLALLib.Graphics.Models.Mesh"/> class.
		/// </summary>
		/// <param name="beginMode">Begin mode.</param>
		/// <param name="dataHint">Data hint.</param>
		/// <param name="indexHint">Index hint.</param>
		public Mesh (BeginMode beginMode, BufferUsageHint dataHint, BufferUsageHint indexHint)
		{
			MeshType = beginMode;
			DataBufferUsageHint = dataHint;
			IndexBufferUsageHint = indexHint;

			// Generate GL names
			GL.GenVertexArrays(1, out Vao);
			GL.GenBuffers(1, out Vbo);
			GL.GenBuffers(1, out Ebo);

			// bind the vao
			GL.BindVertexArray(Vao);

			// Bind Vertex buffer to vao
			GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);

			// Set the vbo settings in the vao
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(Data), 0);

			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(Data), Vector3.SizeInBytes);

			GL.EnableVertexAttribArray(2);
			GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, BlittableValueType.StrideOf(Data), Vector3.SizeInBytes * 2);

			// Bind index buffer to vao
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);

			// Unbind the vao
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Uploads the indices to the GPU. Use only if you need to upload only the indices. If you need to upload index AND vertex data, use Upload() instead.
		/// </summary>
		public virtual void UploadIndices()
		{
			// bind the vao
			GL.BindVertexArray(Vao);

			// Upload indices
			UploadIndicesFaster ();

			// Unbind the vao
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Uploads the indices to the GPU.
		/// </summary>
		protected virtual void UploadIndicesFaster()
		{
			// Element array object (indices)
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(Indices.Length * sizeof(uint)), Indices, IndexBufferUsageHint);
		}

		/// <summary>
		/// Uploads the vertices to the GPU. Use only if you need to upload only the vertices. If you need to upload index AND vertex data, use Upload() instead.
		/// </summary>
		public virtual void UploadVertices()
		{
			// bind the vao
			GL.BindVertexArray(Vao);

			// Upload vertices
			UploadVerticesFaster ();

			// Unbind the vao
			GL.BindVertexArray(0);
		}

		protected virtual void UploadVerticesFaster()
		{
			// Vertex buffer object (mesh vertex data)
			GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(Data.Length * MeshElement.SizeInBytes), Data, DataBufferUsageHint);

			// Unbind the vao
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Upload index AND vertex data to the GPU. Call this function only if you need to upload BOTH. If you only need to upload 
		/// vertices OR indices alone, use UploadIndices() or UploadVertices() instead.
		/// </summary>
		public void Upload()
		{
			// bind the vao
			GL.BindVertexArray(Vao);

			UploadVerticesFaster ();
			UploadIndicesFaster ();

			// Unbind the vao
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Render the mesh.
		/// </summary>
		public virtual void Render()
		{
			// bind the vao
			GL.BindVertexArray(Vao);

			// Draw it
			GL.DrawRangeElements(MeshType, 0, Indices.Length-1, Indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			// unbind the vao
			GL.BindVertexArray(0);
		}

		/// <summary>
		/// Optimizes the vertex and index data in many different ways.
		/// </summary>
		/// <param name="smoothNormals">If set to <c>true</c> smooth-optimize normals. The end result might not be what you intended, 
		/// use only if you know you want to do it. More info: 
		/// http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-9-vbo-indexing/#Shared_vs_Separate</param>
		public virtual void OptimizeData(bool smoothNormals = false)
		{
			// TODO: Implement vertex cache optimizing

			// If normal smoothing optimization is set, do it
			if (smoothNormals)
			{
				// The algorithm here takes all normals in the same point in space, and adds 
				// them together and then normalizes the result and overwrites old normals of
				// the point in space. This results in more duplicate elements if there are
				// cornered normals, and removes hard corners from polygon normals.

				// The normals will not be added together, if texture coordinates of the elements
				// differ from each other.

				// http://www.opengl-tutorial.org/intermediate-tutorials/tutorial-9-vbo-indexing/#Shared_vs_Separate

				// List to store indices that are already modified
				List<uint> modified = new List<uint>();

				// Loop through all elements
				for (uint i = 0; i < Data.Length; i++) 
				{
					// Check if element of current index has already been modified
					if(!modified.Contains(i))
					{
						// It has not been modified yet

						// Store indices of current duplicate vertices (with differing normals)
						List<uint> currentIndices = new List<uint>();

						// Get current element
						MeshElement element = Data [i];

						// Add current index to the current indices list
						currentIndices.Add (i);
						modified.Add (i); // Unnecessary, but it keeps the statistics in tact

						// Go through the rest of the elements
						for (uint o = i+1; o < Data.Length; o++) 
						{
							// Get the second element
							MeshElement secondElement = Data [o];

							// Compare current element and second element if they have same position in space and same texture coordinates
							if(element.Vertex == secondElement.Vertex &&
							   element.TexCoord == secondElement.TexCoord)
							{
								// Add the second normal vector to the current element's normal vector
								element.Normal += secondElement.Normal;

								// Add the index to the modified list and current indices list
								modified.Add (o);
								currentIndices.Add (o);
							}
						}

						// After all normal vectors have been added, normalize the vector to make it proper normal vector again
						element.Normal.Normalize ();

						// Set all duplicate elements as the modified element
						foreach (var item in currentIndices) {
							Data [item] = element;
						}
					}
				}

				//Console.WriteLine ("Smoothing optimization modified {0} elements.", modified.Count);
			}

			// Dictionary to connect element with index
			Dictionary<MeshElement, uint> data = new Dictionary<MeshElement, uint> ();

			// Lists holding the new indices and elements
			List<uint> indices = new List<uint> ();
			List<MeshElement> elements = new List<MeshElement> ();

			// Find duplicate elements
			foreach (var index in Indices)
			{
				// Check if element already exists in the dictionary
				if(data.ContainsKey(Data[index]))
				{
					// It does! Get the index from dictionary
					uint i = data [Data [index]];

					// Add the index pointing to the already existing element to the list
					indices.Add (i);
				}
				else
				{
					// Element does not already exist.

					//Add element to the dictionary
					data.Add (Data [index], index);

					// Add the index to the list
					indices.Add (index);
				}
			}

			// Remove elements that has no index pointing to them
			uint dataOffset = 0;
			for (uint i = 0; i < Data.Length; i++) 
			{
				// Check if data index is in the indices list
				if(indices.Contains(i - dataOffset))
				{
					// This element must be preserved

					elements.Add (Data [i]);
				}
				else
				{
					// This element must be removed.

					// Update indices by decrementing 1 if index is larger than current element index
					for (int o = 0; o < indices.Count; o++) 
					{
						if(indices[o] > (i - dataOffset))
						{
							indices [o]--;
						}
					}

					// Increase offset to compensate missing element
					dataOffset++;
				}
			}

			//Console.WriteLine ("Optimization removed {0} elements", dataOffset);
			//Console.WriteLine ("Before: {0} elements, {1} indices", Data.Length, Indices.Length);
			//Console.WriteLine ("After: {0} elements, {1} indices", elements.Count, indices.Count);

			// Now the indices should be pointing to unique vertices

			// Use new indices and elements as arrays
			Data = elements.ToArray ();
			Indices = indices.ToArray ();
		}

		#region IDisposable
		// Implement IDisposable
		protected bool Disposed = false;
		public void Dispose()
		{
			Dispose(true);
		}
		protected virtual void Dispose (bool manual)
		{
			if (!Disposed) {

				if (manual) 
				{
					GL.DeleteBuffers (1, ref Vbo);
					GL.DeleteBuffers (1, ref Ebo);
					GL.DeleteVertexArrays (1, ref Vao);
					Disposed = true;
					GC.SuppressFinalize(this);
				}
				else
				{
					GC.KeepAlive(ResourceDisposer.DisposingQueue);
					ResourceDisposer.DisposingQueue.Enqueue(this);
					Console.WriteLine("Warning: Mesh not unloaded manually!");
				}


			}
		}
		~Mesh()
		{
			Dispose (false);
		}
		#endregion
	}
}

