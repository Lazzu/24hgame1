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
using hgame1.Gamearea;
using hgame1.Graphics.Rendering;
using hgame1.CharacterEntities;
using hgame1.Graphics.Lightning;

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
		                                       "genocide simulator 2013", 
		                                       GameWindowFlags.Default, 
		                                       DisplayDevice.Default,
		                                       3, 3, 
		                                       GraphicsContextFlags.Default)
		{
			// Set current settings
			Settings.CurrentSettings = settings;

			// Camera initialization
			Camera.Init (this);

			Camera.MoveAtInstantly (new Vector2 ((Width / 2), (Height / 2)));

			// Texture manager initialization
			TextureManager.Init (this);

			// Initialize spritedrawer
			SpriteDrawer.Initialize (this);

			// Initialize GUI
			Gui.Initialize (this);

			VSync = VSyncMode.On;
		}

		Sprite sprite;

		Label lblFPS;

        GameArea Gamearea;

		PostEffectFrameBuffer FxFbo;

		Light playerLight;

		protected override void OnLoad (EventArgs e)
		{
			GL.Enable (EnableCap.DepthTest);
			GL.Enable (EnableCap.Blend);
			GL.Enable (EnableCap.VertexProgramPointSize);
			GL.Enable (EnableCap.CullFace);
			GL.Enable (EnableCap.ScissorTest);

			base.OnLoad (e);

			TextureManager.LoadTexture("default.png", "default");
            TextureManager.LoadTexture("basicearth.png", "basicearth");
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

            this.Gamearea = new GameArea(100, 100);
            this.Gamearea.AddPlayer("Pelaaja", 100, new Vector2(25, 25), 0);
            this.Gamearea.Playercharacters[0].setLocation(new Vector2((Width / 2), (Height / 2)));

			FxFbo = new PostEffectFrameBuffer (new Point(Size));

			playerLight = new Light ();

			playerLight.Color = new Vector3 (1, 1, 1);
			playerLight.Intensity = 100;
			playerLight.Position = new Vector3 (0, 0, 0);

			LightningEngine.Lights.Add (playerLight);
		}

		Random r = new Random ();
		double totalTime = 0;
		protected override void OnUpdateFrame (FrameEventArgs e)
		{
			totalTime += e.Time;

			base.OnUpdateFrame (e);

            if (Keyboard[OpenTK.Input.Key.A])
            {
                //Camera.Move(new Vector2(1, 0) / (float)e.Time);
                this.Gamearea.Playercharacters[0].Move(new Vector2(-5f,0));
            }

            if (Keyboard[OpenTK.Input.Key.D])
            {
                //Camera.Move(new Vector2(-1, 0) / (float)e.Time);
                this.Gamearea.Playercharacters[0].Move(new Vector2(5f, 0));
            }

            if (Keyboard[OpenTK.Input.Key.W])
            {
                //Camera.Move(new Vector2(0, 1) / (float)e.Time);
                this.Gamearea.Playercharacters[0].Move(new Vector2(0, -5f));
            }

            if (Keyboard[OpenTK.Input.Key.S])
            {
                //Camera.Move(new Vector2(0, -1) / (float)e.Time);
                this.Gamearea.Playercharacters[0].Move(new Vector2(0, 5f));
            }

            Camera.MoveAtInstantly(this.Gamearea.Playercharacters[0].CharacterLocation - new Vector2(Width/2,Height/2));

			playerLight.Position = new Vector3 (this.Gamearea.Playercharacters[0].CharacterLocation);

			if (Keyboard [OpenTK.Input.Key.Space])
                Camera.MoveAtInstantly(new Vector2(Width / 2, Height / 2) / (float)e.Time);

			for(int i=0; i<100; i++)
			{
				SpriteDrawData drawdata = new SpriteDrawData ();

				drawdata.Color = new Vector4 (1, 1, 1, 1);
				drawdata.Texdata = new Vector3 (sprite.TextureCoordinates.X, sprite.TextureCoordinates.Y, sprite.Size);
				drawdata.TranslateData = new Vector3 ((float)(Math.Sin(totalTime + i)) * 160, (float)(Math.Cos(totalTime + i)) * 100, (float)r.NextDouble());
				//drawdata.TranslateData = new Vector3 (640, 400, (float)r.NextDouble());

				//Console.WriteLine (drawdata.TranslateData);

				SpriteDrawer.AddSprite (sprite, drawdata);
			}

            foreach(Character character in Gamearea.Playercharacters){
                character.Draw();
            }

            this.Gamearea.MapTiles.Draw();

		}

		double fpsTime;
		int fpsCount;

		protected override void OnRenderFrame (FrameEventArgs e)
		{
			GL.Scissor(0,0,Width,Height);

			// Clear the main framebuffer
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			if(FxFbo != null)
			{
				FxFbo.StartRender ();
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			}

			base.OnRenderFrame (e);



			if(FxFbo != null){	
				FxFbo.StopRender ();
				FxFbo.RenderOnScreen ();
			}

			Gui.Render (e);

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

