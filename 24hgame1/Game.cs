using System;
using OpenTK;
using OpenTK.Graphics;
using hgame1.Graphics.Sprites;
using hgame1.Graphics.Textures;
using hgame1.Graphics;
using hgame1.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;
using hgame1.Graphics.GUI;
using hgame1.Graphics.GUI.Controllers;
using System.Drawing;

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

			// Initialize GUI
			Gui.Initialize (this);
		}

		Sprite sprite;

		Label lblFPS;

		protected override void OnLoad (EventArgs e)
		{
			GL.Enable (EnableCap.DepthTest);
			GL.Enable (EnableCap.Blend);
			GL.Enable (EnableCap.VertexProgramPointSize);
			GL.Enable (EnableCap.CullFace);
			GL.Enable (EnableCap.ScissorTest);

			base.OnLoad (e);

			TextureManager.LoadTexture("default.png", "default");
			ShaderProgramManager.LoadXml ("sprite", "sprite.shader");

			sprite = new Sprite(TextureManager.Get("default"), ShaderProgramManager.Get("sprite"), 100, new Vector2(0,0) );

			// Gui stuff must be initialized before GameWindow.OnLoad()
			lblFPS = new Label();
			lblFPS.Z = int.MaxValue - 1;
			lblFPS.Visible = true;
			lblFPS.GrabInput = false;
			lblFPS.Font = new Font (FontFamily.GenericMonospace, 14, FontStyle.Bold);
			lblFPS.Value = "FPS: Calculating...";
			Gui.Add (lblFPS);


		}

		Random r = new Random ();

		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			base.OnUpdateFrame (e);



			for(int i=0; i<1000; i++)
			{
				SpriteDrawData drawdata = new SpriteDrawData ();

				drawdata.Color = new Vector4 (1, 1, 1, 1);
				drawdata.Texdata = new Vector3 (sprite.TextureCoordinates.X, sprite.TextureCoordinates.Y, sprite.Size);
				drawdata.TranslateData = new Vector3 ((float)(Math.Sin(e.Time * i) + 0.5) * 1280, (float)(Math.Sin(e.Time * i) + 0.5) * 800, (float)r.NextDouble());
				//drawdata.TranslateData = new Vector3 (640, 400, (float)r.NextDouble());

				//Console.WriteLine (drawdata.TranslateData);

				SpriteDrawer.AddSprite (sprite, drawdata);
			}


		}

		double fpsTime;
		int fpsCount;

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			base.OnRenderFrame (e);

			SwapBuffers ();

			ErrorCode ec = GL.GetError();
			if (ec != 0)
			{
				throw new System.Exception(ec.ToString());
			}

			fpsCount++;
			fpsTime += e.Time;

			if(fpsTime > 1.0)
			{
				fpsTime -= 1.0;
				lblFPS.Value = "FPS: " + fpsCount;
				fpsCount = 0;
			}
		}


	}
}

