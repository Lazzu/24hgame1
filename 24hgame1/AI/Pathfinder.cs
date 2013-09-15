using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using hgame1.AI.Properties;
using hgame1.Tilemap;
using hgame1.Tilemap.Properties;

namespace hgame1.AI
{
    public class Pathfinder
    {
        
        private Tile[,] currentTileMap;
        private int TileSize;

        public List<Node> closedList;
        public List<Node> openList;

        public Node TranslateToNode(int _x, int _y)
        {
            Node tempNode = new Node(_x,_y,1,1);
            
            return tempNode;
        }

        public void setCurrenttilemapinfo(Tile[,] _tilemap, int _tilesize)
        {
            this.currentTileMap = _tilemap;
            this.TileSize = _tilesize;
        }

        public List<Node> getPath(Node _startpoint, Node _endpoint)
        {
            this.closedList.Clear();
            this.openList.Clear();
            this.openList.Add(_startpoint);
            while (openList.Count > 0)
            {

                // Get node with smallest cost in open list;
                Node q = new Node(1, 1);
                float ftest = float.MaxValue;
                foreach (Node tempnode in openList)
                {
                    if (tempnode.F < ftest)
                    {
                        ftest = tempnode.F;
                        q = tempnode;
                    }
                }
                openList.Remove(q);

                List<Node> successors = new List<Node>();
                // Generate possible nextnodes.
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        Node tempnode = new Node(q.X - 1 + j, q.Y - 1 + k);
                        if ((this.currentTileMap[tempnode.X, tempnode.Y].Tileproperty == Tiletype.Floor) ||
                            (this.currentTileMap[tempnode.X, tempnode.Y].Tileproperty == Tiletype.Door) ||
                            (this.currentTileMap[tempnode.X, tempnode.Y].Tileproperty == Tiletype.FloorCeiling))
                        {
                            tempnode.set_g((int)(q.G + Math.Abs((q.X - tempnode.X) + (q.Y - tempnode.Y))));
                            tempnode.set_h((int)Math.Abs(_endpoint.X - tempnode.X + _endpoint.Y - tempnode.Y));
                            tempnode.calculate_f();
                            successors.Add(tempnode);
                        }
                    }
                }

                // Go through successors
                foreach(Node successornode in successors){
                    bool skip = false;
                    if(successornode.X == _endpoint.X && successornode.Y == _endpoint.Y){
                        this.openList.Clear();
                        skip = true;
                    }
                    foreach(Node againstopen in openList)
                    {
                        if((againstopen.X == successornode.X) && (againstopen.Y == successornode.Y)){
                            if (againstopen.F > successornode.F)
                            {
                                foreach (Node againstclosed in closedList)
                                {
                                    if ((againstclosed.X == successornode.X) && (againstclosed.Y == successornode.Y))
                                    {
                                        if (againstclosed.F < successornode.F)
                                        {
                                            skip = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                skip = true;
                            }
                        }
                    }
                    if(!skip){
                        openList.Add(successornode);
                    }
                }


                // Finally
                closedList.Add(q);
            }
            return this.closedList;
        }
    }
}
