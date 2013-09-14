using System;

namespace hgame1.Graphics
{
	public class GraphicsSettings
	{
		public int Width {
			get;
			set;
		}

		public int Height {
			get;
			set;
		}

		public int MSAA {
			get;
			set;
		}

		public GraphicsSettings ()
		{
			Width = 1280;
			Height = 800;
			MSAA = 8;
		}
	}
}

