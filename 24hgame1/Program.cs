using System;
using hgame1.Utilities;
using System.IO;
using hgame1.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;
using hgame1.Graphics.GUI;
using hgame1.Graphics.GUI.Drawers;

namespace hgame1
{
	public static class Program
	{
		[STAThread]
		public static void Main(string[] arg)
		{
			bool OverwriteWithDefaultSettings = true;

			if(!File.Exists("settings.xml") || OverwriteWithDefaultSettings)
			{
				Settings s = new Settings ();
				Xml.Write.File (s, "settings.xml"); // Overwrite defaults.
			}

			// The settings.xml should always exist at this point

			using (Game game = new Game(Xml.Read.ReadFile<Settings>("settings.xml")))
			{
				game.Run ();
			}
		}
	}
}

