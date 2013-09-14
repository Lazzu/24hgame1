using System;
using OpenTK.Input;
using hgame1.Graphics.Shaders;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.GUI.Controllers.Containers
{
	public class GuiControllerContainer : GuiController
	{
		public Container Children = new Container();

		public GuiControllerContainer ()
		{
			Children.Parent = this;
		}

		public override void Render (FrameEventArgs e, Box2 bounds)
		{
			base.Render (e, bounds);

			Box2 childrenArea = new Box2 (Position, Size - ChildrenOffset);

			Console.WriteLine ("Children drawing area: {0}", childrenArea);

			Children.Render (e, childrenArea);
		}

		public override void Update (FrameEventArgs e)
		{
			base.Update (e);

			Children.Update (e);
		}

		public override void Load ()
		{
			base.Load ();

			Children.Load ();
		}

		public override void MouseButtonDown (MouseButtonEventArgs e)
		{
			base.MouseButtonDown (e);

			Children.MouseButtonDown (e);
		}

		public override void MouseButtonUp (MouseButtonEventArgs e)
		{
			base.MouseButtonUp (e);

			Children.MouseButtonUp (e);
		}

		public override void MouseMove (MouseMoveEventArgs e)
		{
			base.MouseMove (e);

			Children.MouseMove (e);
		}

		public override void MouseWheel (MouseWheelEventArgs e)
		{
			base.MouseWheel (e);

			Children.MouseWheelChanged (e);
		}
	}

	public class GuiControllerContainer<T> : GuiController<T>
	{
		public Container Children = new Container();

		public GuiControllerContainer ()
		{
			Children.Parent = this;
		}

		public override void Render (FrameEventArgs e, Box2 bounds)
		{
			//GL.Scissor((int)this.Position.X, (int)this.Position.Y, (int)this.Size.X, (int)this.Size.Y);

			base.Render (e, bounds);

			Box2 childrenArea = new Box2 (Position, Size - ChildrenOffset);

			Console.WriteLine ("Children drawing area: {0}", childrenArea);

			Children.Render (e, childrenArea);
		}

		public override void Update (FrameEventArgs e)
		{
			base.Update (e);

			Children.Update (e);
		}

		public override void Load ()
		{
			base.Load ();

			Children.Load ();
		}

		public override void MouseButtonDown (MouseButtonEventArgs e)
		{
			base.MouseButtonDown (e);

			Children.MouseButtonDown (e);
		}

		public override void MouseButtonUp (MouseButtonEventArgs e)
		{
			base.MouseButtonUp (e);

			Children.MouseButtonUp (e);
		}

		public override void MouseMove (MouseMoveEventArgs e)
		{
			base.MouseMove (e);

			Children.MouseMove (e);
		}

		public override void MouseWheel (MouseWheelEventArgs e)
		{
			base.MouseWheel (e);

			Children.MouseWheelChanged (e);
		}

	}
}

