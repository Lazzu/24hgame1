using System;
using OpenTK;
using hgame1.Graphics.Textures;
using System.Drawing;


namespace hgame1.Graphics.GUI.Controllers
{
    public class Label : GuiController<string>
    {
		protected Texture TextTexture;
		internal Texture Texture {
			get {
				return TextTexture;
			}
			set {
				TextTexture = value;
			}
		}

		public Font Font {
			get {
				return TextDrawer.Font;
			}
			set {
				TextDrawer.Font = value;
				ReDraw ();
			}
		}

		Vector2 maxSize;
		public Vector2 MaxSize {
			get {
				return maxSize;
			}
			set {
				maxSize = value;
			}
		}

		protected Text TextDrawer;

		public Label ()
		{
			maxSize = new Vector2(9999);
			Texture = new Texture ();
			TextDrawer = new Text (TextTexture);
			TextDrawer.Font = new Font (Gui.Settings.DefaultFont, Gui.Settings.DefaultFontSize);

			this.ValueChangedEvent += HandleValueChangedEvent;
			this.ResizedEvent += HandleResizedEvent;

			ControllerValue = ID.ToString();
		}

		void HandleResizedEvent ()
		{
			ReDraw ();
		}

		void HandleValueChangedEvent (string newValue)
		{
			ReDraw ();
		}

		void ReDraw ()
		{
			TextDrawer.MaxWidth = MaxSize.X;
			TextDrawer.WriteText (ControllerValue);
			ControllerSize = new Vector2(TextDrawer.Size.Width, TextDrawer.Size.Height);
		}
    }
}
