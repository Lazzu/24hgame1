using System;
using OpenTK;
using System.Runtime.InteropServices;

namespace hgame1.Graphics.Sprites
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SpriteDrawData
	{
		public Vector3 TranslateData;	// vec2 position + float angle
		public Vector4 Color;			// RGBA
		public Vector3 Texdata;			// vec2 texture coordinates + float sprite size

		public static int SizeInBytes {
			get {
				return Vector3.SizeInBytes + Vector4.SizeInBytes + Vector3.SizeInBytes;
			}
		}
	}
}

