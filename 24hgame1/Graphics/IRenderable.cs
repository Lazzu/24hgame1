using System;
using OpenTK;

namespace hgame1.Graphics
{
	public interface IRenderable
	{
		void QueueRender(ref Matrix4 modelMatrix);
		void Render(ref Matrix4 modelMatrix);
		void RawRender();
		void Update(double time);
	}
}

