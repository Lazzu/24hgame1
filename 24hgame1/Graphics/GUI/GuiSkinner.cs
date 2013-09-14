using System;
using System.Xml.Serialization;

namespace hgame1.Graphics.GUI
{
	[XmlRoot]
	public class GuiSkinner
	{
		public string Controller {
			get;
			set;
		}

		public string Skinner {
			get;
			set;
		}

		public GuiDrawerSettings Settings {
			get;
			set;
		}
	}
}

