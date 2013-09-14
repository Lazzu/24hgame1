using System;
using System.Collections.Generic;
using OpenTK;
using hgame1.Graphics.GUI.Controllers;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.GUI.Drawers;
using hgame1.Utilities;
using OpenTK.Graphics.OpenGL;
using hgame1.Graphics.GUI.Controllers.Windows;

namespace hgame1.Graphics.GUI
{
    public static class Gui
    {
		internal static Container Controllers = new Container ();

		public static GuiController ActiveController;

        public static bool MouseOnGui { get; set; }

		public static GuiSettings Settings {
			get;
			set;
		}

		static GameWindow gw;

		public static Matrix4 GuiProjection;

		static hgame1.Graphics.GUI.Controllers.Image MouseCursor;

		public static void Add(GuiController controller)
		{
			Controllers.AddChild (controller);
		}

		public static void Remove(GuiController controller)
		{
			Controllers.RemoveChild (controller);
		}

        public static void Initialize (GameWindow gameWindow)
        {
			Settings = Xml.Read.ReadFile<GuiSettings>("gui.xml");

			Settings.Shaders.Load ();

			GuiDrawerManager.LoadSettings (Settings.Skinners.ToArray());

			gw = gameWindow;

			gw.Resize += HandleResize;
			gw.Load += GameWindowLoad;
			gw.UpdateFrame += GameWindowUpdateFrame;
			gw.RenderFrame += HandleRenderFrame;
			gw.Mouse.Move += MouseMove;
			gw.Mouse.ButtonDown += MouseButtonDown;
			gw.Mouse.ButtonUp += MouseButtonUp;
			gw.Mouse.WheelChanged += MouseWheelChanged;
			gw.KeyUp += HandleKeyUp;
			gw.KeyDown += HandleKeyDown;
			gw.KeyPress += HandleKeyPress;

			//MouseCursor = new hgame1.Graphics.GUI.Controllers.Image ();
			//MouseCursor.Visible = true;
			//MouseCursor.GrabInput = false;
			//MouseCursor.Z = int.MaxValue;
			//MouseCursor.Value = "gui/" + Settings.MouseSkin + "/mouse/mouse.png";

			//Gui.Add (MouseCursor);
        }

        static void HandleRenderFrame (object sender, FrameEventArgs e)
        {
			Render (e);
        }

        static void HandleResize (object sender, EventArgs e)
        {
			GuiProjection = Matrix4.CreateOrthographicOffCenter (0, gw.Width, gw.Height, 0, -1, 1);
        }

        static void HandleKeyPress (object sender, KeyPressEventArgs e)
        {
			if (ActiveController == null)
				return;


        }

        static void HandleKeyDown (object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
			if (ActiveController == null)
				return;

        }

        static void HandleKeyUp (object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
			if (ActiveController == null)
				return;

        }

        static void MouseWheelChanged (object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
			Controllers.MouseWheelChanged (e);
        }

        static void MouseButtonUp (object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
			Controllers.MouseButtonUp (e);
        }

        static void MouseButtonDown (object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
			ActiveController = null;

			Controllers.MouseButtonDown (e);
        }

        static void MouseMove (object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
			MouseOnGui = false;

			//MouseCursor.Position = new Vector2 (e.Position.X, e.Position.Y);

			Controllers.MouseMove (e);
        }

        static void GameWindowUpdateFrame (object sender, FrameEventArgs e)
        {
			Controllers.Update (e);
        }

        static void GameWindowLoad (object sender, EventArgs e)
        {


			Controllers.Load ();
        }

        public static void Render (FrameEventArgs e)
        {
			GL.Disable (EnableCap.DepthTest);
			GL.Enable (EnableCap.Blend);
			GL.Enable (EnableCap.ScissorTest);

			// Set the correct blending function
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			// Whole screen as drawing area
			//GL.Scissor(0, 0, gw.Width, gw.Height);

			Controllers.Render (e, new Box2(0, 0, gw.Width, gw.Height));

			GL.Enable (EnableCap.DepthTest);
        }

		static Box2 clipArea;

		/// <summary>
		/// Gets or sets the clipping area of the GUI elements. Used for clipping the children of containers.
		/// </summary>
		/// <value>The clipping area.</value>
		public static Box2 ClipArea {
			get {
				return CorrectClipArea(clipArea);
			}
			set {
				// Transfer the area from Gui coordinates to OpenGL coordinates
				Box2 tmp = CorrectClipArea(value);

				// Check if the clipping area has changed from the last set
				if(Math.Abs (tmp.Top - clipArea.Top) > 0.9 &&
					Math.Abs (tmp.Left - clipArea.Left) > 0.9 &&
					Math.Abs (tmp.Right - clipArea.Right) > 0.9 &&
					Math.Abs (tmp.Bottom - clipArea.Bottom) > 0.9)
				{
					// Clip the area
					GL.Scissor((int)tmp.Left, (int)tmp.Top, (int)tmp.Right, (int)tmp.Bottom);

					// Save the area for the next compare
					clipArea = tmp;
				}
			}
		}

		static Box2 CorrectClipArea(Box2 area)
		{
			area.Top = gw.Height - area.Top - area.Bottom;

			return area;
		}

    }
}
