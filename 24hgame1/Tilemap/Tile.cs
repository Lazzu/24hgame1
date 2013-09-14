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

        //Auxilary tile properties
        public Walltype Tilewalltype { get; private set; }
        public Doorstate Tiledoorstate { get; private set; }
        public Doortype Tiledoortype { get; private set; }
        public Doordirection Tiledoordirection { get; private set; }
        public WindowState Tilewindowstate { get; private set; }

        /// <summary>
        /// basictile constructor.
        /// </summary>
        public Tile()
        {
            this.Tileproperty = Tiletype.Floor;
            this.Floortexturename = "basictile";
            this.Walltexturename = "basictile";
            this.Ceilingtexturename = "basictile";
        }
        
        /// <summary>
        /// Floortile constructor.
        /// </summary>
        public Tile(string _floortexture)
        {
            this.Tileproperty = Tiletype.Floor;
            this.Floortexturename = _floortexture;
        }

        /// <summary>
        /// Floortile with ceiling constructor.
        /// </summary>
        public Tile(string _floortexture, string _ceilingtexture)
        {
            this.Tileproperty = Tiletype.FloorCeiling;
            this.Floortexturename = _floortexture;
            this.Ceilingtexturename = _ceilingtexture;
        }

        /// <summary>
        /// Walltile constructor.
        /// </summary>
        public Tile(Walltype _walltype, string _walltexture, string _ceilingtexture)
        {
            this.Tileproperty = Tiletype.Wall;
            this.Tilewalltype = _walltype;
            this.Walltexturename = _walltexture;
            this.Ceilingtexturename = _ceilingtexture;
        }

        /// <summary>
        /// Wall with window constructor.
        /// </summary>
        public Tile(WindowState _windowstate, string _walltexture)
        {
            this.Tileproperty = Tiletype.WallWindow;
            this.Tilewindowstate = _windowstate;
            this.Walltexturename = _walltexture;
        }

        /// <summary>
        /// Door constructor
        /// </summary>
        public Tile(Doortype _doortype, Doorstate _doorstate, Doordirection _doordirection, string _floorTexture)
        {
            this.Tileproperty = Tiletype.Door;
            this.Tiledoortype = _doortype;
            this.Tiledoorstate = _doorstate;
            this.Tiledoordirection = _doordirection;
            this.Floortexturename = _floorTexture;
        }
    }
}
