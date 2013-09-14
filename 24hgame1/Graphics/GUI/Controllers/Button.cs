using System;
using OpenTK;

namespace hgame1.Graphics.GUI.Controllers
{
	public class Button : Label
	{
		protected Vector4 ButtonPadding;
		public Vector4 Padding {
			get {
				return ButtonPadding;
			}
			set {
				ButtonPadding = value;
				Resize ();
			}
		}

		public Button ()
		{
			ResizedEvent += HandleResizedEvent;
			MovedEvent += HandleMovedEvent;
			ValueChangedEvent += HandleValueChangedEvent;
			//MouseClickEvent += HandleMouseClickEvent;

			ControllerActivateable = true;
			Padding = new Vector4 (5,5,5,5);
		}

		void HandleResizedEvent ()
		{
			Resize ();
		}

		void HandleMovedEvent ()
		{
			Resize ();
		}

		void HandleMouseClickEvent (OpenTK.Input.MouseButton button)
		{
			//Console.WriteLine ("Clicked a button!");
		}

		void HandleValueChangedEvent (string newValue)
		{
			Resize ();
		}

		void Resize ()
		{
			ControllerSize = TextTexture.Size + new Vector2(Padding.X + Padding.Z, Padding.Y + Padding.W);
		}
	}
}

