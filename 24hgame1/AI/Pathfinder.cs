using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using hgame1.AI.Properties;

namespace hgame1.AI
{
    public class Pathfinder
    {
        public int[,] PathMap { get; private set; }

        public List<Node> closedList;
        public List<Node> openList;

        public void setPathMap(int[,] _pathmap)
        {
            this.PathMap = _pathmap;
        }

        public Node TranslateToNode(float _x, float _y)
        {
            Node tempNode;
            tempNode.set_x(_x);
            tempNode.set_y(_y);

            return tempNode;
        }

        public List<Node> getPath(Node _startpoint, Node _endpoint)
        {
            return this.closedList;
        }
    }
}
