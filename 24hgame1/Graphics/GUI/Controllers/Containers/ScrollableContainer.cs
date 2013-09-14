using System;
using OpenTK;

namespace hgame1.Graphics.GUI.Controllers.Containers
{
	public class ScrollableContainer : GuiControllerContainer<Vector2>
	{
		public Vector2 ScrollValue {
			get {
				return ControllerValue;
			}
			set {
				// Fires the valueChanged event
				Value = value;
			}
		}
		public ScrollableContainer ()
		{
			this.MouseWheelEvent += HandleMouseWheelEvent;
		}

		void HandleMouseWheelEvent (OpenTK.Input.MouseWheelEventArgs e)
		{
			// Fires the value changed event
			Vector2 tmp = new Vector2(ControllerValue.X, ControllerValue.Y - e.DeltaPrecise);

			if (tmp.Y < 0)
				tmp.Y = 0;



			Value = tmp;
		}
	}
}

