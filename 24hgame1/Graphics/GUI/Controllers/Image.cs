using System;
using hgame1.Graphics.Textures;

namespace hgame1.Graphics.GUI.Controllers
{
	public class Image : GuiController<string>
	{
		protected Texture ImageTexture;
		internal Texture Texture {
			get {
				return ImageTexture;
			}
			set {
				ImageTexture = value;
			}
		}

		public Image ()
		{
			this.ValueChangedEvent += HandleValueChangedEvent;

			ControllerValue = "default.png";

			Reload ();
		}

		void HandleValueChangedEvent (string newValue)
		{
			Reload ();
		}

		void Reload ()
		{
			if(ImageTexture != null)
				ImageTexture.Dispose ();

			ImageTexture = null;

			ImageTexture = TextureManager.LoadTexture (ControllerValue, "Gui.Image." + ID.ToString() + "." + ControllerValue);
			ControllerSize = ImageTexture.Size;
		}
	}
}

