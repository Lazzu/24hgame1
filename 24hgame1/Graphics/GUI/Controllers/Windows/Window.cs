using System;
using hgame1.Graphics.GUI.Controllers.Containers;
using hgame1.Graphics.Textures;
using System.Drawing;
using OpenTK;
using OpenTK.Input;

namespace hgame1.Graphics.GUI.Controllers.Windows
{
	public class Window : ScrollableContainer
	{
		protected string WindowTitle;
		public string Title {
			get {
				return WindowTitle;
			}
			set {
				WindowTitle = value;
				ReDraw ();
			}
		}

		public int TitleHeight {
			get;
			set;
		}

		public int BorderWidth {
			get;
			set;
		}

		internal Texture TitleTexture {
			get;
			set;
		}

		protected Text TextDrawer;

		protected bool MouseOverTitleBar = false;

		public Window ()
		{
			ControllerActivateable = true;

			TitleHeight = 14;
			BorderWidth = 3;

			TitleTexture = new Texture ();
			TextDrawer = new Text (TitleTexture);
			TextDrawer.Font = new Font (Gui.Settings.DefaultFont, Gui.Settings.DefaultFontSize);

			WindowTitle = "Window " + ID.ToString();

			ReDraw ();

			ResizedEvent += HandleResizedEvent;
			//MouseClickEvent += HandleMouseClickEvent;
			MouseButtonUpEvent += HandleMouseButtonUpEvent;
			MouseButtonDownEvent += HandleMouseButtonDownEvent;
			MouseLeaveEvent += HandleMouseLeaveEvent;
			MouseEnterEvent += HandleMouseEnterEvent;
			MouseHoverEvent += HandleMouseHoverEvent;
		}

		void HandleResizedEvent ()
		{
			//ControllerSize += new Vector2 (BorderWidth * 2, TitleHeight + BorderWidth * 3);
		}

		void HandleMouseClickEvent (MouseButton button)
		{
			//Console.WriteLine ("Clicked a window!");
		}

		void HandleMouseButtonUpEvent (MouseButton button)
		{
			ControllerDragged = false;
		}

		void HandleMouseButtonDownEvent (MouseButton button)
		{
			ControllerDragged = false;
			if(button == MouseButton.Left && MouseOverTitleBar && Gui.ActiveController == this)
			{
				ControllerDragged = true;
			}
		}

		void HandleMouseHoverEvent (MouseMoveEventArgs e)
		{
			MouseOverTitleBar = false;

			if(GraphicsHelpers.PointInsideBox(
				new Box2(ControllerPosition, ControllerPosition + new Vector2(ControllerSize.X, TitleHeight + BorderWidth * 2)), 
				new Vector2(e.Position.X, e.Position.Y)
			))
			{
				MouseOverTitleBar = true;
			}

			if(ControllerDragged && MouseOverTitleBar)
			{
				ControllerPosition += new Vector2 (e.XDelta, e.YDelta);
			}
		}

		void HandleMouseLeaveEvent ()
		{
			MouseOverTitleBar = false;
			ControllerDragged = false;
		}

		void HandleMouseEnterEvent ()
		{
			// Näytä ruksi?
		}

		void ReDraw ()
		{
			TextDrawer.WriteText (WindowTitle);
			TitleTexture.Size = new Vector2(TextDrawer.Size.Width, TextDrawer.Size.Height);
		}
	}
}

