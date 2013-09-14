using System;
using OpenTK;
using System.Collections.Generic;
using hgame1.Graphics.Shaders;
using OpenTK.Input;
using hgame1.Graphics.GUI.Drawers;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.GUI.Controllers
{
	public class Container
	{
		public delegate void ChildAddedEvent (GuiController ctrl);

		public event ChildAddedEvent ChildAdded;

		protected List<GuiController> ContainerChildren = new List<GuiController>();
		protected List<GuiController> RemoveList = new List<GuiController>();

		public GuiController Parent {
			get;
			set;
		}

		/// <summary>
		/// Gets the children list of the container.
		/// </summary>
		/// <value>The children.</value>
		public List<GuiController> Children {
			get {
				return ContainerChildren;
			}
		}

		public Container ()
		{
		}

		public void AddChildren(IEnumerable<GuiController> ctrls)
		{
			foreach (var item in ctrls) {
				AddChild (item);
			}
		}

		public void AddChild(GuiController ctrl)
		{
			// Get drawer using type
			string type = ctrl.GetType ().ToString ();
			IGuiDrawer drawer = GuiDrawerManager.GetDrawer (type);

			// Set drawer and add to list
			ctrl.Drawer = drawer;
			ctrl.Parent = Parent;
			ContainerChildren.Add (ctrl);

			// Sort list
			ContainerChildren.Sort ();
		}

		public void RemoveChildren (IEnumerable<GuiController> ctrls, bool hard = false)
		{
			foreach (var item in ctrls) {
				RemoveChild (item, hard);
			}

		}

		public void RemoveChild(GuiController ctrl, bool hard = false)
		{
			if(hard)
			{
				ContainerChildren.Remove (ctrl);
			}
			else
			{
				RemoveList.Add (ctrl);
			}

		}

		public void Render (FrameEventArgs e, Box2 bounds)
		{
			foreach (var child in ContainerChildren) {
				child.Render (e, bounds);
			}
		}

		public void Update (FrameEventArgs e)
		{
			foreach (var child in ContainerChildren) {
				child.Update (e);
			}

			RemoveChildren (RemoveList, true);
			RemoveList.Clear ();
		}

		public void Load ()
		{
			foreach (var child in ContainerChildren) {
				child.Load ();
			}
		}

		public void MouseButtonUp (MouseButtonEventArgs e)
		{
			foreach (var controller in ContainerChildren) 
			{
				Vector2 offset = Vector2.Zero;

				if(controller.Parent != null)
				{
					offset = controller.Parent.ChildrenOffset + controller.Parent.Position;
				}

				if( // If mouse is inside the controller box
				   GraphicsHelpers.PointInsideBox(
					new Box2(
					offset + controller.Position, 
					offset + controller.Position + controller.Size + controller.ChildrenOffset), 
					new Vector2(
					e.Position.X, 
					e.Position.Y)
					)
				   ) 
				{
					controller.MouseButtonUp (e);
				}
			}
		}

		public void MouseButtonDown (MouseButtonEventArgs e)
		{
			foreach (var controller in ContainerChildren) 
			{
				Vector2 offset = Vector2.Zero;

				if(controller.Parent != null)
				{
					offset = controller.Parent.ChildrenOffset + controller.Parent.Position;
				}

				if( // If mouse is inside the controller box
				   GraphicsHelpers.PointInsideBox(
					new Box2(
						offset + controller.Position, 
						offset + controller.Position + controller.Size + controller.ChildrenOffset), 
					new Vector2(e.Position.X, e.Position.Y)
					)
				   ) 
				{
					controller.MouseButtonDown (e);
				}
			}
		}

		public void MouseMove (MouseMoveEventArgs e)
		{
			foreach (var controller in ContainerChildren)
			{
				Vector2 offset = Vector2.Zero;

				if(controller.Parent != null)
				{
					offset = controller.Parent.ChildrenOffset + controller.Parent.Position;
				}

				if( // If mouse is inside the controller box
				   GraphicsHelpers.PointInsideBox(
					new Box2(
					offset + controller.Position, 
					offset + controller.Position + controller.Size), 
					new Vector2(e.Position.X, e.Position.Y)
					)
				   ) 
				{
					// Check if GUI should grab input preventing the rest of the program using the input
					if(controller.GrabInput)
						Gui.MouseOnGui = true;

					controller.Hovered = true;
					controller.MouseMove (e);

				}
				else if (controller.Hovered) // If the controller was hovered on last mouse move
				{
					// The controller is not being hovered
					controller.Hovered = false;
				}
			}
		}

		public void MouseWheelChanged (MouseWheelEventArgs e)
		{
			foreach (var controller in ContainerChildren) 
			{
				Vector2 offset = Vector2.Zero;

				if(controller.Parent != null)
				{
					offset = controller.Parent.ChildrenOffset + controller.Parent.Position;
				}

				if( // If mouse is inside the controller box
				   GraphicsHelpers.PointInsideBox(
					new Box2(
					offset + controller.Position, 
					offset + controller.Position + controller.Size + controller.ChildrenOffset), 
					new Vector2(
					e.Position.X, 
					e.Position.Y)
					)
				   ) 
				{
					controller.MouseWheel (e);
				}
			}
		}
	}
}

