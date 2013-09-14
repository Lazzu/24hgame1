using hgame1.Graphics.Shaders;
using hgame1.Graphics.GUI.Controllers;

namespace hgame1.Graphics.GUI.Drawers
{
    public interface IGuiDrawer
    {
		void Initialize (GuiDrawerSettings settings);
		void Draw (GuiController obj);
    }
}
