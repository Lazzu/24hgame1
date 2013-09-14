using System;
using System.Collections.Generic;
using System.IO;

namespace hgame1.Graphics.Models.Loaders
{
	public interface IModelLoader
	{
		Mesh[] LoadModel (string fileName);
		Mesh[] LoadModel (Stream stream);
	}
}

