using System;
using OpenTK;
using OpenTK.Graphics;
using hgame1.Graphics.Sprites;

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

			// Initialize spritedrawer
			SpriteDrawer.Initialize (this);
		}

		protected override void OnLoad (EventArgs e)
		{
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

