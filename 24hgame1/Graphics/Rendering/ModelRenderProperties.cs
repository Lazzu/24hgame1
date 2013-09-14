using System;
using System.Collections.Generic;
using hgame1.Graphics.Textures;
using hgame1.Graphics.Models;
using OpenTK;

namespace hgame1.Graphics.Rendering
{
	public class ModelRenderProperties
	{
		public Model Model;

		public Matrix4 ModelMatrix;

		public ModelRenderProperties (Model model, ref Matrix4 modelMatrix)
		{
			Model = model;
			ModelMatrix = modelMatrix;
		}
	}
}

