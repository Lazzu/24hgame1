using System;
using System.Xml.Serialization;
using OpenTK;
using System.Drawing;
using hgame1.Utilities;

namespace hgame1.Graphics.GUI.Drawers
{
	[XmlType("WindowDrawerSettings")]
	public class WindowDrawerSettings : GuiDrawerSettings
	{
		[XmlElement]
		public Vector4 BackgroundColor {
			get;
			set;
		}

		[XmlElement]
		public Vector4 BorderColor {
			get;
			set;
		}

		public WindowDrawerSettings ()
		{
			BackgroundColor = new Vector4 (1.0f);
			BorderColor = new Vector4 (1.0f);
		}
	}
}

