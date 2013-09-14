﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.Tilemap
{
    class Tilemap
    {
        //tilemap
        public Tile[,] tilemap {get; private set;}

        private int Tilemapwidth, Tilemapheight;

        public Tilemap(int _tilemapwidth,int _tilemapheight){
            this.tilemap = new Tile[_tilemapwidth, _tilemapheight];
            this.Tilemapwidth = _tilemapwidth;
            this.Tilemapheight = _tilemapheight;
        }

        public void Populatetilemap(){
            for (int i = 0; i < this.Tilemapheight; i++ )
            {
                for (int j = 0; j < this.Tilemapwidth; )
                {
                    this.tilemap[i, j] = new Tile();
                }
            }
        }
    }
}