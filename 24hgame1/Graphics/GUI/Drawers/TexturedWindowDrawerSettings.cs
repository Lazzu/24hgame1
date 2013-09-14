using System;
using System.Xml.Serialization;

namespace hgame1.Graphics.GUI.Drawers
{
	[XmlType("TexturedWindowDrawerSettings")]
	public class TexturedWindowDrawerSettings : GuiDrawerSettings
	{
		[XmlElement("Skin")]
		public string Skin {
			get;
			set;
		}

		public TexturedWindowDrawerSettings ()
		{
			Skin = "default";
		}
	}
}

