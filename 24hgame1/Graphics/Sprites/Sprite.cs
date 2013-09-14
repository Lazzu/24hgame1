using System;
using hgame1.Graphics.Textures;
using hgame1.Graphics.Shaders;
using OpenTK;

namespace hgame1.Graphics.Sprites
{
	public class Sprite
	{
		public Texture Texture;
		public ShaderProgram Shader;

		public Vector2 TextureCoordinates;

		public int Size;

		public Sprite (Texture t, ShaderProgram s, int sz, Vector2 tc)
		{
			Texture = t;
			Shader = s;
			Size = sz;
			TextureCoordinates = tc;
		}
	}
}

