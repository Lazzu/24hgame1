using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.CharacterEntities.Properties;
using hgame1.AI.Properties;
using hgame1.AI;
using OpenTK;
using hgame1.Graphics.Sprites;
using hgame1.Graphics.Textures;
using hgame1.Graphics.Shaders;


namespace hgame1.CharacterEntities
{
    class Character
    {
        public CharacterControltype Charactertype { get; private set; }
        public Sprite CharacterSprite { get; private set; }

        // generic variables
        public Vector2 CharacterLocation { get; private set; }
        public float CharacterDirection { get; private set; }
        public string CharacterName { get; private set; }
        public int CharacterHealth { get; private set; }

        //auxilary properties and variables
        public AIentity CharacterAI;


        /// <summary>
        /// Player character constructor.
        /// </summary>
        public Character(string _name, int _health ,Vector2 _location, float _direction)
        {
            this.Charactertype = CharacterControltype.Player;
            this.CharacterName = _name;
            this.CharacterLocation = _location;
            this.CharacterDirection = _direction;
            this.CharacterHealth = _health;
            this.CharacterSprite = new Sprite(TextureManager.Get("default"), ShaderProgramManager.Get("sprite"), 100, new Vector2(0, 0));
        }

        /// <summary>
        /// AI constructor
        /// </summary>
        public Character(string _name, int _health ,Vector2 _location, float _direction, AItype _aitype, AIstate _aistate)
        {
            this.Charactertype = CharacterControltype.AI;
            this.CharacterAI = new AIentity(_aistate, _aitype);

            this.CharacterName = _name;
            this.CharacterHealth = _health;
            this.CharacterLocation = _location;
            this.CharacterDirection = _direction;
        }

        public void setDirection(float _direction)
        {
            this.CharacterDirection = _direction;
        }

        public void setLocation(Vector2 _location)
        {
            this.CharacterLocation = _location;
        }

        public void Move(Vector2 _moveamount)
        {
            this.CharacterLocation = this.CharacterLocation + _moveamount;
        }

        public void Draw()
        {
            SpriteDrawData characterdrawdata = new SpriteDrawData();
            characterdrawdata.Color = new Vector4(1, 1, 1, 1);
            characterdrawdata.Texdata = new Vector3(this.CharacterSprite.TextureCoordinates.X, this.CharacterSprite.TextureCoordinates.Y, 100);
            characterdrawdata.TranslateData = new Vector3(this.CharacterLocation.X, this.CharacterLocation.Y, 0);

            SpriteDrawer.AddSprite(this.CharacterSprite, characterdrawdata);
        }
    }
}
