using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.AI.Properties;
using hgame1.CharacterEntities;

namespace hgame1.AI
{
    class AIentity
    {
        public AIstate State { get; private set; }
        public AItype Type { get; private set; }

        public Character BrainedEntity { get; private set;}

        public AIentity(AIstate _state, AItype _type)
        {
            this.State = _state;
            this.Type = _type;
        }
    }
}
