using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace hgame1.Graphics.Shaders
{
	[XmlType("ShaderProgram")]
	public class ShaderProgramXml
	{
		[XmlAttribute]
		public string Name {
			get;
			set;
		}

		public List<ShaderXml> Shaders {
			get;
			set;
		}

		[XmlArrayItem("Uniform")]
		public List<string> Uniforms {
			get;
			set;
		}

		public ShaderProgramXml ()
		{
			Shaders = new List<ShaderXml> ();
			Uniforms = new List<string> ();
		}
	}
}

