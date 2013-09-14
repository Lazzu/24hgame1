using System;
using System.Collections.Generic;
using OpenTK;
using System.IO;

namespace hgame1.Graphics.Models.Loaders.WavefrontObj
{
	public class ModelLoader : IModelLoader
	{
		List<Mesh> meshes = new List<Mesh>();

		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> texcoord = new List<Vector2>();
		List<int> vIndices = new List<int>();
		List<int> nIndices = new List<int>();
		List<int> tIndices = new List<int>();

		//ObjectModelMaterialLoader materials = new ObjectModelMaterialLoader();

		string currentMaterial = string.Empty;

		//bool triangle = true; // Is mesh is made up with polygons with more vertices than three

		public Mesh[] LoadModel(string fileName)
		{
			StreamReader file = new StreamReader( fileName );

			Load (file);

			file.Close();

			Mesh[] ret = meshes.ToArray ();

			meshes.Clear ();

			return ret;
		}

		public Mesh[] LoadModel(Stream stream)
		{
			StreamReader file = new StreamReader( stream );

			Load (file);

			file.Close();

			Mesh[] ret = meshes.ToArray ();

			meshes.Clear ();

			return ret;
		}

		private void Load(StreamReader file)
		{
			Mesh m;
			string line;
			while((line = file.ReadLine()) != null)
			{
				// Convert line in parts, replace tabs with space and dual spaces with one space, then split with one space
				string[] parts = line.Replace('	', ' ').Replace("  ", " ").Split(' ');
				float x,y,z;
				int a,b,c;
				int i,count;

				switch(parts[0]){
					case "v":

					float.TryParse(parts[1], out x);
					float.TryParse(parts[2], out y);
					float.TryParse(parts[3], out z);

					vertices.Add(new Vector3(x,y,z));

					continue;
					case "vt":

					float.TryParse(parts[1], out x);
					float.TryParse(parts[2], out y);

					texcoord.Add(new Vector2(x,y));

					continue;
					case "vn":

					float.TryParse(parts[1], out x);
					float.TryParse(parts[2], out y);
					float.TryParse(parts[3], out z);

					normals.Add(new Vector3(x,y,z));

					continue;
					case "f":

					count = parts.Length;

					/*
					if(count > 4)
					{
						triangle = false;
					}
					else if(count == 4)
					{
						triangle = true;
					}
					*/

					for(i=1; i < count; i++)
					{
						string[] trianglepart = parts[i].Split('/');

						int.TryParse(trianglepart[0], out a);
						int.TryParse(trianglepart[1], out b);
						int.TryParse(trianglepart[2], out c);

						vIndices.Add(a-1);
						tIndices.Add(b-1);
						nIndices.Add(c-1);
					}

					continue;
					case "mtllib":

					//materials.LoadMaterial(parts[1]);

					continue;
					case "usemtl":

					if(currentMaterial != string.Empty)
					{
						m = MakeMesh();
						//m.Triangles = triangle;
						meshes.Add(m);
						m=null;
					}

					currentMaterial = parts[1];

					vIndices.Clear();
					nIndices.Clear();
					tIndices.Clear();

					continue;
					default:
					Console.WriteLine("Unhandled input from model: " + line);
					continue;
				}

			}

			m = MakeMesh();
			//m.Triangles = triangle;
			meshes.Add(m);
		}

		private Mesh MakeMesh ()
		{
			Mesh m = new Mesh ();

			/*
			if (materials.materials.ContainsKey (currentMaterial)) {
				m.Material = materials.materials [currentMaterial];
			} else {
				Console.WriteLine("No material " + currentMaterial + " found when loading model.");
			}
			*/

			List<uint> meshIndices = new List<uint>();
			List<MeshElement> elements = new List<MeshElement> ();

			for(int i=0; i < vIndices.Count; i++)
			{
				MeshElement e = new MeshElement ();

				e.Vertex = vertices [vIndices [i]];
				e.Normal = normals [nIndices [i]];

				if(texcoord.Count > 0)
					e.TexCoord = texcoord [tIndices [i]];

				//int index = elements.LastIndexOf(e);

				//if( index < 0 )
				{
					meshIndices.Add((uint)i);
					elements.Add (e);
				}
				/*else
				{
					meshIndices.Add ((uint)index);
				}*/
			}

			m.Data = elements.ToArray ();
			m.Indices = meshIndices.ToArray ();

			elements.Clear ();
			meshIndices.Clear ();
			vertices.Clear ();
			normals.Clear ();
			texcoord.Clear ();
			vIndices.Clear ();
			nIndices.Clear ();
			tIndices.Clear ();

			m.OptimizeData ();

			return m;
		}
	}
}

