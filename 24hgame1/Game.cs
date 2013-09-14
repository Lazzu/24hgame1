using System;
using OpenTK;
using OpenTK.Graphics;
using hgame1.Graphics.Sprites;
using hgame1.Graphics.Textures;
using hgame1.Graphics;
using hgame1.Graphics.Shaders;

namespace hgame1
{
	public class Game : GameWindow
	{
		public Game (Settings settings) : base(settings.Graphics.Width, settings.Graphics.Height, 
		                                       new GraphicsMode(
			GraphicsMode.Default.ColorFormat, 
			GraphicsMode.Default.Depth, 
			GraphicsMode.Default.Stencil, 
			settings.Graphics.MSAA ),
		                                       "Linear Quest", 
		                                       GameWindowFlags.Default, 
		                                       DisplayDevice.Default,
		                                       3, 3, 
		                                       GraphicsContextFlags.Default)
		{
			// Set current settings
			Settings.CurrentSettings = settings;

			// Camera initialization
			Camera.Init (this);

			// Texture manager initialization
			TextureManager.Init (this);

			// Initialize spritedrawer
			SpriteDrawer.Initialize (this);
		}

		Sprite sprite;

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			TextureManager.LoadTexture("default.png", "default");
			ShaderProgramManager.LoadXml ("sprite", "sprite.shader");

			sprite = new Sprite(TextureManager.Get("default"), ShaderProgramManager.Get("sprite"), 100, new Box2() );
		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);

			SpriteDrawData drawdata = new SpriteDrawData ();

			drawdata.Color = new Vector4 (1, 1, 1, 1);
			drawdata.TranslateData = new Vector3 (0, 0, 0);
			drawdata.Texdata = new Vector3 (0, 0, sprite.Size);

			SpriteDrawer.AddSprite (sprite, drawdata);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
		}


	}
}

