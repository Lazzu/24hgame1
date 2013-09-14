using System;
using System.Xml.Serialization;
using hgame1.Graphics.GUI.Drawers;

namespace hgame1.Graphics.GUI
{
	[XmlInclude(typeof(TexturedWindowDrawerSettings))]
	[XmlInclude(typeof(WindowDrawerSettings))]
	[XmlInclude(typeof(ButtonDrawerSettings))]
	public abstract class GuiDrawerSettings
	{

	}
}

