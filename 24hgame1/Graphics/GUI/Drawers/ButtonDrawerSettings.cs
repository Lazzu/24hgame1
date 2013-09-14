using System;
using System.Xml.Serialization;
using OpenTK;

namespace hgame1.Graphics.GUI.Drawers
{
	[XmlType("ButtonDrawerSettings")]
	public class ButtonDrawerSettings : GuiDrawerSettings
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

		[XmlElement]
		public Vector4 HoverColor {
			get;
			set;
		}

		[XmlElement]
		public double CornerRoundness {
			get;
			set;
		}

		[XmlElement]
		public int CornerSlices {
			get;
			set;
		}

		public float FadeAmount {
			get;
			set;
		}

		public ButtonDrawerSettings ()
		{
			BackgroundColor = new Vector4 (0.2f, 0.3f, 0.2f, 1f);
			BorderColor = new Vector4 (0.1f, 0.1f, 0.1f, 1f);
			HoverColor = new Vector4 (0.6f, 0.6f, 0.3f, 1f);
			FadeAmount = 0.05f;
			CornerRoundness = 5;
			CornerSlices = 5;
		}
	}
}

