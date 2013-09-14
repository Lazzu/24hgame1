using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using hgame1.Graphics.GUI.Events;
using hgame1.Graphics.GUI.Drawers;
using hgame1.Graphics.Shaders;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.GUI.Controllers
{
	// Controller events
	public delegate void VisibilityChangedEvent ();
	public delegate void MoveEvent ();
	public delegate void ResizeEvent ();
	public delegate void LoadEvent ();
	public delegate void UpdateEvent (FrameEventArgs e);
	public delegate void RenderEvent (FrameEventArgs e);
	public delegate void DestroyEvent ();

	// Input events

	// Mouse events
	public delegate void MouseEnterEvent ();
	public delegate void MouseHoverEvent (MouseMoveEventArgs e);
	public delegate void MouseLeaveEvent ();
	public delegate void MouseButtonDownEvent (MouseButton button);
	public delegate void MouseButtonUpEvent (MouseButton button);
	public delegate void MouseClickEvent (MouseButton button);
	public delegate void MouseDoubleClickEvent (MouseButton button);
	public delegate void MouseWheelEvent (MouseWheelEventArgs e);
	public delegate void DraggedEvent(MouseMoveEventArgs e);

	// Keyboard events
	public delegate void KeyDownEvent (OpenTK.Input.Key key);
	public delegate void KeyUpEvent (OpenTK.Input.Key key);
	public delegate void KeyPushedEvent (OpenTK.Input.Key key);

	// Yes it is a class, but it works like interface
	public abstract class GuiController : IComparable<GuiController>
	{
		public event VisibilityChangedEvent VisibilityChangedEvent;
		public event MoveEvent MovedEvent;
		public event ResizeEvent ResizedEvent;
		public event UpdateEvent UpdatedEvent;
		public event RenderEvent BeforeRenderEvent;
		public event LoadEvent LoadingEvent;
		public event DestroyEvent DestroyedEvent;

		public event MouseEnterEvent MouseEnterEvent;
		public event MouseHoverEvent MouseHoverEvent;
		public event MouseLeaveEvent MouseLeaveEvent;
		public event MouseButtonDownEvent MouseButtonDownEvent;
		public event MouseButtonUpEvent MouseButtonUpEvent;
		public event MouseClickEvent MouseClickEvent;
		public event MouseDoubleClickEvent MouseDoubleClickEvent;
		public event MouseWheelEvent MouseWheelEvent;
		public event DraggedEvent DraggedEvent;

		public event KeyDownEvent KeyDownEvent;
		public event KeyUpEvent KeyUpEvent;
		public event KeyPushedEvent KeyPushedEvent;

		/// <summary>
		/// Gets or sets the drawer to be used to draw this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/>.
		/// </summary>
		/// <value>The drawer.</value>
		public IGuiDrawer Drawer
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the ID of the <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/>.
		/// </summary>
		/// <value>The ID.</value>
		public Guid ID {
			get;
			protected set;
		}

		public GuiController Parent {
			get;
			set;
		}

		protected Vector2 ControllerPosition;

		/// <summary>
		/// Gets or sets the position of this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/>.
		/// </summary>
		/// <value>The position.</value>
		public Vector2 Position
		{
			get
			{
				return ControllerPosition;
			}
			set
			{
				ControllerPosition = value;

				if (MovedEvent != null)
					MovedEvent ();
			}
		}

		protected Vector2 ControllerSize;

		/// <summary>
		/// Gets or sets the size of this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/>.
		/// </summary>
		/// <value>The size.</value>
		public Vector2 Size
		{
			get
			{
				return ControllerSize;
			}
			set
			{
				ControllerSize = value;

				if (ResizedEvent != null)
					ResizedEvent ();
			}
		}

		protected bool ControllerVisible = true;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/> is visible.
		/// </summary>
		/// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
		public bool Visible
		{
			get
			{
				return ControllerVisible;
			}
			set
			{
				ControllerVisible = value;

				if (VisibilityChangedEvent != null)
					VisibilityChangedEvent ();
			}
		}

		protected bool ControllerDragged = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/> is being dragged.
		/// </summary>
		/// <value><c>true</c> if being dragged; otherwise, <c>false</c>.</value>
		public bool Dragged {
			get {
				return ControllerDragged;
			}
			set {
				ControllerDragged = value;
			}
		}

		bool lasthovered = false;
		protected bool ControllerHovered = false;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/> is hovered by mouse.
		/// </summary>
		/// <value><c>true</c> if hovered; otherwise, <c>false</c>.</value>
		public bool Hovered {
			get {
				return ControllerHovered;
			}
			set {
				// Set value
				ControllerHovered = value;

				// Check if we enter or leave the controller
				if (lasthovered != ControllerHovered){
					if (ControllerHovered && MouseEnterEvent != null)
						MouseEnterEvent ();
					else if(MouseLeaveEvent != null)
						MouseLeaveEvent ();
				}

				// Save hover state for next time
				lasthovered = ControllerHovered;
			}
		}

		protected bool ControllerActivateable;

		/// <summary>
		/// Gets a value indicating whether this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/> is activateable.
		/// </summary>
		/// <value><c>true</c> if activateable; otherwise, <c>false</c>.</value>
		public bool Activateable {
			get {
				return ControllerActivateable;
			}
		}

		protected bool ControllerGrabInput = true;
		public bool GrabInput {
			get {
				return ControllerGrabInput;
			}
			set {
				ControllerGrabInput = value;
			}
		}

		int z;
		public int Z {
			get {
				return z;
			}
			set {
				z = value;
			}
		}

		public Vector2 ChildrenOffset {
			get;
			set;
		}

		protected double CurrentTime = 0;
		protected double CurrentDeltaTime = 0;

		/// <summary>
		/// Render this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/> with the specified FrameEventArgs and shader.
		/// </summary>
		/// <param name="e">FrameEventArgs.</param>
		/// <param name="shader">Shader.</param>
		public virtual void Render (FrameEventArgs e, Box2 bounds)
		{
			CurrentDeltaTime = e.Time;
			CurrentTime += e.Time;

			Gui.ClipArea = bounds;

			//GL.Scissor((int)bounds.Left, (int)bounds.Top, (int)bounds.Right, (int)bounds.Bottom);

			if (BeforeRenderEvent != null)
				BeforeRenderEvent (e);

			if(this.Drawer != null)
				this.Drawer.Draw (this);
		}

		/// <summary>
		/// Load this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/>.
		/// </summary>
		public virtual void Load ()
		{
			if (LoadingEvent != null)
				LoadingEvent ();
		}

		/// <summary>
		/// Update this <see cref="MGLALLib.Graphics.GUI.Controllers.GuiController"/>.
		/// </summary>
		/// <param name="e">E.</param>
		public virtual void Update (FrameEventArgs e)
		{
			if (UpdatedEvent != null)
				UpdatedEvent (e);
		}

		protected bool[] MouseButtons = new bool[15];
		protected double LastClickTime;

		public virtual void MouseButtonDown(MouseButtonEventArgs e)
		{
			// Set the state as down
			MouseButtons [(int)e.Button] = true;

			// If this controller can be activated
			if(ControllerActivateable)
			{
				// Set this controller as active controller
				Gui.ActiveController = this;
			}

			if (MouseButtonDownEvent != null)
				MouseButtonDownEvent (e.Button);
		}

		public virtual void MouseButtonUp(MouseButtonEventArgs e)
		{
			// Get the last up/down state
			bool down = MouseButtons [(int)e.Button];

			// Set the current up/down state as up
			MouseButtons [(int)e.Button] = false;

			// If button was previously down on this controller
			if (down) 
			{
				// Get the interval in between this and the last click
				double clickInterval = CurrentTime - LastClickTime;

				// Check if we should fire double click or single click event
				if(clickInterval > 0 && clickInterval <= Gui.Settings.DoubleClickInterval)
				{
					if(MouseDoubleClickEvent != null)
						MouseDoubleClickEvent (e.Button);

					if(MouseClickEvent != null)
						MouseClickEvent (e.Button);
				}
				else
				{
					if(MouseClickEvent != null)
						MouseClickEvent (e.Button);
				}

				// Set the last click time as current time
				LastClickTime = CurrentTime;

			}

			// Fire the button up event
			if (MouseButtonUpEvent != null)
				MouseButtonUpEvent (e.Button);
		}

		public virtual void MouseWheel (MouseWheelEventArgs e)
		{
			if (MouseWheelEvent != null)
				MouseWheelEvent (e);
		}

		public virtual void MouseMove(MouseMoveEventArgs e)
		{
			// Fire the Hover event
			if (MouseHoverEvent != null && ControllerHovered)
				MouseHoverEvent (e);

			if (DraggedEvent != null && ControllerDragged)
				DraggedEvent (e);
		}

		protected GuiController ()
		{
			ID = Guid.NewGuid ();
		}

		#region IComparable implementation

		/// <summary>
		/// Compares the Controller's Z value.
		/// </summary>
		/// <returns>Greater than 0 if greater than other. Less than 0 if less than other. 0 if equal.</returns>
		/// <param name="other">Other.</param>
		public int CompareTo (GuiController other)
		{
			return Z - other.Z;
		}

		#endregion
	}

	public abstract class GuiController<T> : GuiController
	{
		public delegate void ValueChangeEvent (T newValue);

		public event ValueChangeEvent ValueChangedEvent;

		protected T ControllerValue;
		public T Value
		{
			get
			{
				return ControllerValue;
			}
			set
			{
				ControllerValue = value;

				if (ValueChangedEvent != null)
					ValueChangedEvent (ControllerValue);
			}
		}
	}
}
