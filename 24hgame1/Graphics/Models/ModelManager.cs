using System;
using System.Collections.Generic;
using hgame1.Graphics.Models.Loaders;

namespace hgame1.Graphics.Models
{
	public static class ModelManager
	{
		static Dictionary<string, List<Mesh>> models = new Dictionary<string, List<Mesh>>();

		public static void Add(string name, Model model)
		{
			models.Add (name, model.Meshes);
		}

		public static Model Get(string name)
		{
			if (! models.ContainsKey (name))
				return null;

			return new Model(){
				Meshes = models [name]
			};
		}

		public static void Remove(string name)
		{
			models.Remove (name);
		}

		public static Model Load<T>(string name, string file) where T : IModelLoader, new()
		{
			if (models.ContainsKey (name))
				return Get (name);

			T loader = new T ();

			Model m = new Model();
			m.Meshes.AddRange(loader.LoadModel (file));

			Add (name, m);

			return m;
		}
	}
}

