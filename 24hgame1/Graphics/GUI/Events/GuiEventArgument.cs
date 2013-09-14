using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hgame1.Graphics.GUI.Events
{
    public class GuiEventArgument<T> : EventArgs
    {
        public T NewValue { get; set; }
    }
}
