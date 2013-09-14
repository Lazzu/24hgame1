using System;
using OpenTK.Graphics.OpenGL;
using System.Xml.Serialization;

namespace hgame1.Graphics.Shaders
{
	[XmlType("Shader")]
	public class ShaderXml
	{
		public string Data {
			get;
			set;
		}

		public bool Inline {
			get;
			set;
		}

		public ShaderType Type{
			get;
			set;
		}
	}
}

