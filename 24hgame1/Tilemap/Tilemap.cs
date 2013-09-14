using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.Tilemap
{
    class Tilemap
    {
        //tilemap
        public Tile[,] tilemap {get; private set;}

        public Tilemap(int _tilemapwidth,int _tilemapheight){
            this.tilemap = new Tile[_tilemapwidth, _tilemapheight];
        }

        public void Populatetilemap(){
        }
    }
}
