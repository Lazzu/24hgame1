using System;

namespace hgame1
{
	public static class Program
	{
		[STAThread]
		public static void Main(string[] arg)
		{


			using(Game game = new Game(new Settings()))
			{
				game.Run ();
			}
		}
	}
}

