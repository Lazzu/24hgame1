using System;
using System.Collections.Generic;

namespace hgame1.Graphics.GUI.Drawers
{
	public static class GuiDrawerManager
	{
		public static Dictionary<string, IGuiDrawer> Drawers = new Dictionary<string, IGuiDrawer>();

		// Namespaces to search the types in
		public static List<string> Namespaces = new List<string> {
			"hgame1.Graphics.GUI.Drawers", // TODO: If namespaces change, check this out too if it needs to be changed
			"hgame1.Graphics.GUI.Controllers",
			"hgame1.Graphics.GUI.Controllers.Containers",
			"hgame1.Graphics.GUI.Controllers.Windows",
			"hgame1.Forms",
		};

		/// <summary>
		/// Gets the drawer for GuiComponent type.
		/// </summary>
		/// <returns>The drawer.</returns>
		/// <param name="type">Type.</param>
		public static IGuiDrawer GetDrawer (string type)
		{
			// Get drawer type
			type = GetFullType(type);

			IGuiDrawer gd;
			Drawers.TryGetValue (type, out gd);

			return gd;
		}

		// Load array of skinners
		public static void LoadSettings(GuiSkinner[] settings)
		{
			foreach (var setting in settings) {
				LoadSettings (setting);
			}
		}

		// Load skinner
		public static void LoadSettings(GuiSkinner settings)
		{
			// Find full type names (with namespaces)
			settings.Controller = GetFullType (settings.Controller);
			settings.Skinner = GetFullType (settings.Skinner);

			// Check if the skinner already exists
			if (Drawers.ContainsKey (settings.Controller))
				throw new ApplicationException ("Skinner must be unique!");

			// Get drawer type
			Type drawerType = Type.GetType (settings.Skinner);

			// Check if type was not found
			if(drawerType == null)
			{
				// Go thru all assemblies in current app domain and try to find the type
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) 
				{
					drawerType = assembly.GetType (settings.Skinner);

					if (drawerType != null)
						break;
				}
			}

			// Create drawer instance
			IGuiDrawer instance = (IGuiDrawer)Activator.CreateInstance(drawerType);

			// Initialize the drawer instance
			instance.Initialize (settings.Settings);

			// Add drawer to the dictionary
			Drawers.Add (settings.Controller, instance);
		}

		static string GetFullType(string type)
		{
			string nstype = type;

			// Try to get type
			Type t = Type.GetType (nstype);

			// Check if type was not found
			if(t == null)
			{
				// Go thru all assemblies in current app domain and try to find the type
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) 
				{
					nstype = type;
					Console.WriteLine ("Looking in assembly {0}", assembly.FullName);

					Console.WriteLine ("Looking for {0}", nstype);
					t = assembly.GetType (nstype);

					if(t == null)
					{
						// Go thru all namespaces
						foreach (var ns in Namespaces) 
						{
							nstype = ns + "." + type;

							Console.WriteLine ("Looking for {0}", nstype);

							// Try to get drawer type
							t = assembly.GetType (nstype);

							// Break loop if type has been found
							if (t != null)
								break;
						}
					}



					// Break loop if type has been found
					if (t != null)
						break;

				}
			}

			// Check if type is still null
			if(t == null)
			{
				throw new ApplicationException ("Can not find type " + type);
			}

			return nstype;
		}
	}
}

