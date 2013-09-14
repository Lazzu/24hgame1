using System;

namespace hgame1.Graphics.Sprites
{
	public class SpriteDrawParameters
	{
		public int Offset {
			get;
			set;
		}

		public int Count {
			get;
			set;
		}

		public SpriteDrawParameters (int offset, int count)
		{
			Offset = offset;
			Count = count;
		}
	}
}

