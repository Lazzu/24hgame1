using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.CharacterEntities.Properties;
using hgame1.AI.Properties;
using hgame1.AI;
using OpenTK;

namespace hgame1.CharacterEntities
{
    class Character
    {
        public CharacterControltype Charactertype { get; private set; }

        // generic variables
        public Vector2 CharacterLocation { get; private set; }
        public float CharacterDirection { get; private set; }

        public string CharacterName { get; private set; }

        //auxilary properties and variables
        public AIentity CharacterAI;


        /// <summary>
        /// Player character constructor.
        /// </summary>
        public Character(string _name, Vector2 _location, float _direction)
        {
            this.Charactertype = CharacterControltype.Player;
            this.CharacterName = _name;
            this.CharacterLocation = _location;
            this.CharacterDirection = _direction;
        }

        /// <summary>
        /// AI constructor
        /// </summary>
        public Character(string _name ,Vector2 _location, float _direction, AItype _aitype, AIstate _aistate)
        {
            this.Charactertype = CharacterControltype.AI;
            this.CharacterAI = new AIentity(_aistate, _aitype);

            this.CharacterName = _name;
            this.CharacterLocation = _location;
            this.CharacterDirection = _direction;
        }

        public void setDirection(float _direction)
        {
            this.CharacterDirection = _direction;
        }
    }
}
