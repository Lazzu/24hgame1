using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.CharacterEntities.Properties;
using OpenTK;

namespace hgame1.CharacterEntities
{
    class Character
    {
        public CharacterControltype Charactertype { get; private set; }

        public Vector2 CharacterLocation { get; private set; }
        public float CharacterDirection { get; private set; }



        public Character()
        {

        }

        public void setDirection(float _direction)
        {
            this.CharacterDirection = _direction;
        }
    }
}
