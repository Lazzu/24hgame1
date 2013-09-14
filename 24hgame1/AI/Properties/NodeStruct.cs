using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hgame1.AI.Properties
{
    struct Node
    {
        public Node parent { get; private set; }
        public float x { get; private set; }
        public float y { get; private set; }
        public int f { get; private set; }
        public int g { get; private set; }
        public int h { get; private set; }

        public void set_f()
        {
            this.f = this.g + this.h;
        }

        public void set_h(int _h)
        {
            this.h = _h;
        }

        public void set_x(float _x)
        {
            this.x = _x;
        }

        public void set_y(float _y)
        {
            this.y = _y;
        }

        public void set_parent(ref Node _parent)
        {
            this.parent = parent;
        }
    }
}
