using System;
using OpenTK;
using OpenTK.Graphics;
using hgame1.Graphics.Sprites;
using hgame1.Graphics.Textures;

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

			// Texture manager initialization
			TextureManager.Init (this);

			// Initialize spritedrawer
			SpriteDrawer.Initialize (this);
		}

		protected override void OnLoad (EventArgs e)
		{
			TextureManager.LoadTexture("default.png", "default");

			base.OnLoad (e);

		}

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
		}

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			base.OnRenderFrame (e);
		}


	}
}

