using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.Tilemap.Properties;
using hgame1.Graphics.Sprites;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.Textures;
using OpenTK;
using hgame1.Graphics;
using hgame1.AI;

namespace hgame1.Tilemap
{
    class Tilemap
    {
        //tilemap
        public Tile[,] tilemap {get; private set;}

		public TileSet TileSet { get; set; }

        private int Tilemapwidth, Tilemapheight;

        public Tilemap(int _tilemapwidth,int _tilemapheight, int _tileSize){
            this.tilemap = new Tile[_tilemapwidth, _tilemapheight];
            this.Tilemapwidth = _tilemapwidth;
            this.Tilemapheight = _tilemapheight;
			TileSet = new TileSet { TileSize = _tileSize };
            this.TileSet.Tiles.Add("basicsprite", new Sprite(TextureManager.Get("default"), ShaderProgramManager.Get("sprite"), 100, new Vector2(0,0) ));
        }

        public void Populatetilemap(){
            for (int i = 0; i < this.Tilemapheight; i++ )
            {
                for (int j = 0; j < this.Tilemapwidth; j++ )
                {
                    this.tilemap[i, j] = new Tile();
                }
            }
        }

		public void Draw()
		{
			Box2 bounds = Camera.ScreenBounds;

			int xmin = (int)(bounds.Left / TileSet.TileSize);
			int xmax = (int)(bounds.Right / TileSet.TileSize);

			int ymin = (int)(bounds.Top / TileSet.TileSize);
			int ymax = (int)(bounds.Bottom / TileSet.TileSize);

			if (xmin < 0)
				xmin = 0;

			if (xmax > Tilemapwidth)
				xmax = Tilemapwidth;

			if (ymin < 0)
				ymin = 0;

			if (ymax > Tilemapheight)
				ymax = Tilemapheight;

			for (int i = xmin; i < xmax; i++ )
			{
				for (int j = ymin; j < ymax; j++ )
				{
					Tile tile = tilemap [i, j];

					Sprite floor = TileSet.Tiles [tile.Floorspritename];
					Sprite wall = TileSet.Tiles [tile.Wallspritename];
					Sprite ceiling = TileSet.Tiles [tile.Ceilingspritename];

					SpriteDrawData floordrawdata = new SpriteDrawData ();
					floordrawdata.Color = new Vector4(1,1,1,1);
					floordrawdata.Texdata = new Vector3 (floor.TextureCoordinates.X, floor.TextureCoordinates.Y, TileSet.TileSize);
					floordrawdata.TranslateData = new Vector3 (i*TileSet.TileSize,j*TileSet.TileSize,0);

					SpriteDrawData walldrawdata = new SpriteDrawData ();
					walldrawdata.Color = new Vector4(1,1,1,1);
					walldrawdata.Texdata = new Vector3 (wall.TextureCoordinates.X, wall.TextureCoordinates.Y, TileSet.TileSize);
					walldrawdata.TranslateData = new Vector3 (i*TileSet.TileSize,j*TileSet.TileSize,0);

					SpriteDrawData ceilingdrawdata = new SpriteDrawData ();
					ceilingdrawdata.Color = new Vector4(1,1,1,1);
					ceilingdrawdata.Texdata = new Vector3 (ceiling.TextureCoordinates.X, ceiling.TextureCoordinates.Y, TileSet.TileSize);
					ceilingdrawdata.TranslateData = new Vector3 (i*TileSet.TileSize,j*TileSet.TileSize,0);

					SpriteDrawer.AddSprite (floor, floordrawdata);
					SpriteDrawer.AddSprite (wall, walldrawdata);
					SpriteDrawer.AddSprite (ceiling, ceilingdrawdata);
				}
			}
		}
    }
}
