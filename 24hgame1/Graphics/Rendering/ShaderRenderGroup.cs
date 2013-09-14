using System;
using System.Collections.Generic;
using hgame1.Graphics.Textures;
using hgame1.Graphics.Models;

namespace hgame1.Graphics.Rendering
{
	public class ShaderRenderGroup
	{
		public Dictionary<Texture, List<ModelRenderProperties>> TextureGroups = new Dictionary<Texture, List<ModelRenderProperties>>();
	}
}

