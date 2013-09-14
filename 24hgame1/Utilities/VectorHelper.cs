using System;
using OpenTK;
using System.Drawing;

namespace hgame1.Utilities
{
	public static class VectorHelper
	{
		public static Vector2 Multiply(Vector2 a, Vector2 b)
		{
			return new Vector2 (a.X * b.X, a.Y * b.Y);
		}

		public static Vector3 Multiply(Vector3 a, Vector3 b)
		{
			return new Vector3 (a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		}

		public static Vector4 Multiply(Vector4 a, Vector4 b)
		{
			return new Vector4 (a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
		}

		public static void Multiply(ref Vector2 a, ref Vector2 b, out Vector2 result)
		{
			result = new Vector2 (a.X * b.X, a.Y * b.Y);
		}

		public static void Multiply(ref Vector3 a, ref Vector3 b, out Vector3 result)
		{
			result = new Vector3 (a.X * b.X, a.Y * b.Y, a.Z * b.Z);
		}

		public static void Multiply(ref Vector4 a, ref Vector4 b, out Vector4 result)
		{
			result = new Vector4 (a.X * b.X, a.Y * b.Y, a.Z * b.Z, a.W * b.W);
		}

		public static Vector4 ToVec4(this Color c)
		{
			return new Vector4 (c.R / 255.0f, c.G / 255.0f, c.B / 255.0f, c.A / 255.0f);
		}

		public static Vector3 ToVec3(this Color c)
		{
			return new Vector3 (c.R / 255.0f, c.G / 255.0f, c.B / 255.0f);
		}

		public static Color ToColor(this Vector4 v)
		{
			return Color.FromArgb ((int)(v.W * 255), (int)(v.X * 255), (int)(v.Y * 255), (int)(v.Z * 255));
		}

		public static Color ToColor(this Vector3 v)
		{
			return Color.FromArgb (255, (int)(v.X * 255), (int)(v.Y * 255), (int)(v.Z * 255));
		}
	}
}

