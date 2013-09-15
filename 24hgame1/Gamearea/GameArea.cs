using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hgame1.Tilemap;
using hgame1.AI;
using hgame1.AI.Properties;
using hgame1.CharacterEntities;
using OpenTK;

namespace hgame1.Gamearea
{
    class GameArea
    {
        public Tilemap.Tilemap MapTiles { get; private set; }
        Pathfinder pathfinder = new Pathfinder();

        public List<Character> AIcharacters = new List<Character>();
        public List<Character> Playercharacters = new List<Character>();

        public GameArea(int _gameareawidth, int _gameareaheight)
        {
            this.MapTiles = new Tilemap.Tilemap(_gameareawidth,_gameareaheight, 100);
            this.MapTiles.Populatetilemap();
            this.pathfinder.setCurrenttilemapinfo(this.MapTiles.tilemap, 100);
        }

        public void AddPlayer (string _name, int _health ,Vector2 _location, float _direction)
        {
            this.Playercharacters.Add(new Character(_name, _health, _location, _direction));
        }

        public void AddAiPlayer(string _name, int _health, Vector2 _location, float _direction, AItype _aitype, AIstate _aistate)
        {
            this.AIcharacters.Add(new Character(_name, _health , _location, _direction, _aitype, _aistate));
        }
    }
}
