using System;
using OpenTK;
using hgame1.Graphics.Sprites;

namespace hgame1.Graphics.Particles
{
	public class Particle
	{
		public delegate bool ParticleUpdateDelegate(Particle particle, double time);

		public Vector2 Position;
		public Vector4 Color;
		public float Velocity;
		public float Direction;
		public float Size;
		public float Life;

		public Sprite Sprite;

		public ParticleUpdateDelegate Updaters;

		public bool Update(double time)
		{
			Life -= (float)time;

			if(Life >= 0)
			{
				return false; // Return as dead
			}

			Updaters (this, time);

			return true;
		}

		public void Draw()
		{
			SpriteDrawData drawdata = new SpriteDrawData ();

			drawdata.Color = Color;
			drawdata.Texdata = new Vector3 (Sprite.TextureCoordinates.X, Sprite.TextureCoordinates.Y, Size);
			drawdata.TranslateData = new Vector3 (Position);

			SpriteDrawer.AddSprite (Sprite, drawdata);
		}
	}
}

