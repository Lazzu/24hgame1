using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.Tilemap.Properties;

namespace hgame1.Tilemap
{
    class Tile
    {
        public Tiletype Tileproperty{ get; private set;}

        // Textures used at floor, walls and ceiling;
        public string Floortexturename { get; private set; }
        public string Walltexturename {get; private set; }
        public string Ceilingtexturename {get; private set; }



        //basic constructor that greates basictile
        public Tile()
        {
            this.Tileproperty = Tiletype.Floor;
            this.Floortexturename = "basictile";
            this.Walltexturename = "basictile";
            this.Ceilingtexturename = "basictile";
        }
    }
}
