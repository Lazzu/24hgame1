using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.AI.Properties
{
    public class Node
    {
        public Node parent { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public int F { get; private set; }
        public int G { get; private set; }
        public int H { get; private set; }

        public Node(float _x, float _y, int _g, int _h)
        {
            this.X = _x;
            this.Y = _y;
            this.G = _g;
            this.H = _h;
            this.F = _g + _h;
        }

        public Node(float _x, float _y)
        {
            this.X = _x;
            this.Y = _y;
        }

        public void set_f()
        {
            this.F = this.G + this.H;
        }

        public void set_h(int _h)
        {
            this.H = _h;
        }

        public void set_g(int _g)
        {
            this.G = _g;
        }

        public void set_x(float _x)
        {
            this.X = _x;
        }

        public void set_y(float _y)
        {
            this.Y = _y;
        }


        public void set_parent(ref Node _parent)
        {
            this.parent = parent;
        }
    }
}
