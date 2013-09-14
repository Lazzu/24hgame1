using System;
using hgame1.Graphics.Sprites;

namespace hgame1.Tilemap
{
	public class TileSet
	{
		public SpriteCollection Tiles = new SpriteCollection();
		public int TileSize {
			get;
			set;
		}
	}
}

