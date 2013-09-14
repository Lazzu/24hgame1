using System;
using OpenTK;

namespace hgame1.Graphics
{
	public static class GraphicsHelpers
	{
		public static bool PointInsideBox(Box2 box, float x, float y)
		{
			return PointInsideBox (box, new Vector2 (x, y));
		}

		public static bool PointInsideBox(Box2 box, Vector2 point)
		{
			return point.X > box.Left && 
				point.X < box.Right &&
				point.Y > box.Top &&
				point.Y < box.Bottom;
		}
	}
}

