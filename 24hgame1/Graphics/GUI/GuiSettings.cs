using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Drawing;
using OpenTK;
using hgame1.Graphics.Shaders;

namespace hgame1.Graphics.GUI
{
	[XmlRoot]
	public class GuiSettings
	{
		[XmlElement("DoubleClickInterval")]
		public double DoubleClickInterval {
			get;
			set;
		}

		[XmlElement("Skinners")]
		public List<GuiSkinner> Skinners {
			get;
			set;
		}

		[XmlElement("DefaultFont")]
		public string DefaultFont {
			get;
			set;
		}

		[XmlElement("DefaultFontSize")]
		public int DefaultFontSize {
			get;
			set;
		}

		[XmlElement("MouseSkin")]
		public string MouseSkin {
			get;
			set;
		}

		[XmlElement("SkinColor")]
		public Vector4 Color {
			get;
			set;
		}

		[XmlElement("Shaders")]
		public ShaderProgramCollection Shaders {
			get;
			set;
		}

		public GuiSettings ()
		{
			MouseSkin = "default";
			DoubleClickInterval = 0.5;
			Skinners = new List<GuiSkinner> ();
			DefaultFont = "Courier";
			DefaultFontSize = 8;
			Color = new Vector4(1.0f);
		}
	}
}

