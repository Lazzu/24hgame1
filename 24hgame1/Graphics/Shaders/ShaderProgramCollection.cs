using System;
using System.Collections.Generic;

namespace hgame1.Graphics.Shaders
{
	public class ShaderProgramCollection
	{
		public List<ShaderProgramXml> Programs {
			get;
			set;
		}

		public ShaderProgramCollection ()
		{
			Programs = new List<ShaderProgramXml> ();
		}

		public void Load()
		{
			foreach (var item in Programs) {
				ShaderProgramManager.LoadXml (item);
			}
		}

		public void UnLoad()
		{
			foreach (var item in Programs) {
				ShaderProgramManager.Remove (item);
			}
		}
	}
}

