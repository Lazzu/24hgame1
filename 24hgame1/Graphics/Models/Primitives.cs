using System;
using System.Collections.Generic;
using OpenTK;
using hgame1.Graphics.Models.Loaders.Obj;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.Models
{
	public static class Primitives
	{
		public static Model RoundBox(double width, double height, double cornerRadius, int cornerSlices)
		{
			if (cornerRadius < 0)
				throw new ArgumentException ("Radius of a corner can not be less than zero!");

			if (cornerSlices < 1)
				throw new ArgumentException ("There must be at least one corner slice!");

			List<MeshElement> points = new List<MeshElement> ();
			List<uint> indices = new List<uint> ();

			uint index = 0;

			double degrees = Math.PI / 2 / cornerSlices;
			double degreeOffset = 0;

			Model m = new Model ();

			// Center point
			points.Add (new MeshElement () {
				Vertex = new Vector3(0,0,0),
				Normal = new Vector3(0,0,1),
				TexCoord = new Vector2(0.5f,0.5f)
			});

			indices.Add (index++);

			// Starting offsets
			width = (width / 2);
			height = (height / 2);

			double woffset, hoffset;

			woffset = width;
			hoffset = height;

			float x, y;

			int i;


			// 4 corners
			for(int c = 4; c > 0; c--)
			{
				degreeOffset = c * Math.PI / 2;

				if(c == 0)
				{
					woffset = width;
					hoffset = height;
				}

				if(c == 1)
				{
					woffset = width;
					hoffset = -height;
				}

				if(c == 2)
				{
					woffset = -width;
					hoffset = -height;
				}

				if(c == 3)
				{
					woffset = -width;
					hoffset = height;
				}

				// Create corner points with offset
				for(i=cornerSlices+1; i>0; i--)
				{


					x = (float)Math.Sin ((degrees * i) + degreeOffset);
					y = (float)Math.Cos ((degrees * i) + degreeOffset);

					points.Add (new MeshElement () {
						Vertex = new Vector3((float)((x * cornerRadius) + woffset),(float)((y * cornerRadius) + hoffset),0),
						Normal = new Vector3(0,0,1),
						TexCoord = new Vector2((float)(x), (float)(y)) // TODO: Scale texture coordinates to match with width and height settings
					});

					indices.Add (index++);
				}

			}

			woffset = width;
			hoffset = height;

			degreeOffset = 0;

			i = cornerSlices+1;

			x = (float)Math.Sin ((degrees * i) + degreeOffset);
			y = (float)Math.Cos ((degrees * i) + degreeOffset);

			points.Add (new MeshElement () {
				Vertex = new Vector3((float)((x * cornerRadius) + woffset),(float)((y * cornerRadius) + hoffset),0),
				Normal = new Vector3(0,0,1),
				TexCoord = new Vector2(x,y) // TODO: Scale texture coordinates to match with width and height settings
			});

			indices.Add (index++);

			m.Meshes.Add (new Mesh (BeginMode.TriangleFan, 
			                        BufferUsageHint.StaticDraw, 
			                        BufferUsageHint.StaticDraw) {
				Data = points.ToArray(),
				Indices = indices.ToArray()
			});

			//m.Meshes [0].OptimizeData ();

			return m;



		}

		public static Model Pie(double radius, double degrees, uint slices)
		{

			if (radius <= 0)
				throw new ArgumentException ("Radius of a circle can not be zero or less!");

			if (degrees <= 0)
				throw new ArgumentException ("Degrees must be greater than zero!");

			if (slices < 1)
				throw new ArgumentException ("There must be at least one slice!");

			if (degrees > Math.PI * 2)
				degrees = Math.PI * 2;



			List<MeshElement> points = new List<MeshElement> ();
			List<uint> indices = new List<uint> ();

			uint index = 0;

			// Center point
			points.Add (new MeshElement () {
				Vertex = new Vector3(0,0,0),
				Normal = new Vector3(0,0,1),
				TexCoord = new Vector2(0.5f,0.5f)
			});

			indices.Add (index++);

			// how many degrees per slice?
			degrees = degrees / slices;

			for(uint i=slices+1; i>0; i--)
			{
				float x, y;

				x = (float)Math.Sin (degrees * i);
				y = (float)Math.Cos (degrees * i);

				points.Add (new MeshElement () {
					Vertex = new Vector3((float)(x * radius),(float)(y * radius),0),
					Normal = new Vector3(0,0,1),
					TexCoord = new Vector2(x,y)
				});

				indices.Add (index++);
			}

			Model m = new Model ();

			m.Meshes.Add (new Mesh (BeginMode.TriangleFan, 
			                        BufferUsageHint.StaticDraw, 
			                        BufferUsageHint.StaticDraw) {
				Data = points.ToArray(),
				Indices = indices.ToArray()
			});

			return m;
		}

		public static Model Sphere(double radius, uint rings, uint sectors)
		{
			Mesh meshData = new Mesh ();

			List<MeshElement> data = new List<MeshElement>();
			List<uint> indices = new List<uint>();

			double R = 1.0f/(double)(rings-1);
			double S = 1.0f/(double)(sectors-1);
			uint r, s;

			for (r = 0; r < rings; r++) {
				for (s = 0; s < sectors; s++) {

					double y = Math.Sin (-(Math.PI / 2) + Math.PI * r * R);
					double x = Math.Cos (2 * Math.PI * s * S) * Math.Sin (Math.PI * r * R);
					double z = Math.Sin (2 * Math.PI * s * S) * Math.Sin (Math.PI * r * R);

					MeshElement element = new MeshElement ();

					element.Normal = new Vector3 ((float)x, (float)y, (float)z);
					element.Vertex = new Vector3 ((float)(x * radius), (float)(y * radius), (float)(z * radius));
					element.TexCoord = new Vector2 ((float)(s * S), (float)(r * R));

					data.Add (element);

					uint curRow = r * sectors;
					uint nextRow = (r+1) * sectors;

					indices.Add(curRow + s);
					indices.Add(nextRow + s);
					indices.Add(nextRow + (s+1));

					indices.Add(curRow + s);
					indices.Add(nextRow + (s+1));
					indices.Add(curRow + (s+1));
				}
			}
			/*
			for(r = 0; r < rings-1; r++) {
				for(s = 0; s < sectors-1; s++) {
					indices.Add ((uint)(r * sectors + s));
					indices.Add ((uint)(r * sectors + (s + 1)));
					indices.Add ((uint)((r + 1) * sectors + (s + 1)));
					indices.Add ((uint)((r + 1) * sectors + s));
				}
			}
*/
			meshData.Data = data.ToArray ();
			meshData.Indices = indices.ToArray ();

			data.Clear ();
			indices.Clear ();

			Model model = new Model ();
			model.Meshes.Add (meshData);

			return model;
		}

		public static Model Cube(float size)
		{
			List<MeshElement> data = new List<MeshElement>();
			List<uint> indices = new List<uint>();

			size = size / 2;

			for(int x = -1; x < 2; x=x+2)
			{
				for(int z = -1; z < 2; z=z+2)
				{
					for(int y = -1; y < 2; y=y+2)
					{

						data.Add (new MeshElement {
							Vertex = new Vector3(x*size, y*size, z*size)
						});

					}
				}
			}



			Mesh meshData = new Mesh ();

			meshData.Data = data.ToArray ();
			meshData.Indices = new uint[36];

			/*Front side*/
			meshData.Indices[0]  = (uint)2; meshData.Indices[1]  = (uint)3; meshData.Indices[2]  = (uint)6;
			meshData.Indices[3]  = (uint)6; meshData.Indices[4]  = (uint)3; meshData.Indices[5]  = (uint)7;
			/*Back side*/
			meshData.Indices[6]  = (uint)0; meshData.Indices[7]  = (uint)1; meshData.Indices[8]  = (uint)4;
			meshData.Indices[9]  = (uint)4; meshData.Indices[10] = (uint)1; meshData.Indices[11] = (uint)5;
			/*Left side*/
			meshData.Indices[12] = (uint)0; meshData.Indices[13] = (uint)2; meshData.Indices[14] = (uint)3;
			meshData.Indices[15] = (uint)0; meshData.Indices[16] = (uint)3; meshData.Indices[17] = (uint)1;
			/*Right side*/
			meshData.Indices[18] = (uint)4; meshData.Indices[19] = (uint)6; meshData.Indices[20] = (uint)7;
			meshData.Indices[21] = (uint)4; meshData.Indices[22] = (uint)7; meshData.Indices[23] = (uint)5;
			/*Top*/
			meshData.Indices[24] = (uint)1; meshData.Indices[25] = (uint)3; meshData.Indices[26] = (uint)7;
			meshData.Indices[27] = (uint)1; meshData.Indices[28] = (uint)7; meshData.Indices[29] = (uint)5;
			/*Bottom*/
			meshData.Indices[30] = (uint)0; meshData.Indices[31] = (uint)4; meshData.Indices[32] = (uint)6;
			meshData.Indices[33] = (uint)0; meshData.Indices[34] = (uint)6; meshData.Indices[35] = (uint)2;

			data.Clear ();
			indices.Clear ();

			Model model = new Model ();
			model.Meshes.Add (meshData);

			return model;
		}
	
		public static Model Plane(float x, float y, float x2, float y2) {
			return Plane (new Vector2 (x, y), new Vector2 (x2, y2));
		}
		public static Model Plane(Vector2 p1, Vector2 p2)
		{
			List<MeshElement> data = new List<MeshElement>();
			List<uint> indices = new List<uint>();

			data.Add (new MeshElement () {
				Vertex = new Vector3(p1),
				Normal = new Vector3(0,0,0),
				TexCoord = new Vector2(p1.X, p1.Y),
			});

			data.Add (new MeshElement () {
				Vertex = new Vector3(p1.X, p2.Y, 0),
				Normal = new Vector3(0,0,0),
				TexCoord = new Vector2(p1.X, p2.Y),
			});

			data.Add (new MeshElement () {
				Vertex = new Vector3(p2),
				Normal = new Vector3(0,0,0),
				TexCoord = new Vector2(p2.X, p2.Y),
			});

			data.Add (new MeshElement () {
				Vertex = new Vector3(p2.X, p1.Y, 0),
				Normal = new Vector3(0,0,0),
				TexCoord = new Vector2(p2.X, p1.Y),
			});

			// Square from triangles
			indices.AddRange (new uint[]{
				0,1,2,
				0,2,3
			});

			Mesh meshData = new Mesh ();

			meshData.Indices = indices.ToArray ();
			meshData.Data = data.ToArray ();

			meshData.OptimizeData ();

			Model model = new Model ();
			model.Meshes.Add (meshData);

			return model;
		}
	}
}

