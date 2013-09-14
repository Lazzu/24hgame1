using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.Tilemap;

namespace hgame1.Gamearea
{
    class GameArea
    {
        public Tilemap.Tilemap MapTiles { get; private set; }

        GameArea(int _gameareawidth, int _gameareaheight)
        {
            this.MapTiles = new Tilemap.Tilemap(_gameareawidth,_gameareaheight);
            this.MapTiles.Populatetilemap();
        }
    }
}
