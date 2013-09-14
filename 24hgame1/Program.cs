using System;
using hgame1.Utilities;

namespace hgame1
{
	public static class Program
	{
		[STAThread]
		public static void Main(string[] arg)
		{
			Settings s = new Settings ();

			Xml.Write.File (s, "settings.xml"); // Overwrite defaults. Comment out to actually use the settings file.

			using (Game game = new Game(Xml.Read.ReadFile<Settings>("settings.xml")))
			{
				game.Run ();
			}
		}
	}
}

