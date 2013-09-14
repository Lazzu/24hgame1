using System;
using hgame1.Graphics.GUI.Controllers.Windows;
using hgame1.Graphics.GUI.Controllers;
using OpenTK;
using System.Drawing;

namespace hgame1.Graphics.GUI
{
	[Flags]
	public enum MessageBoxButton
	{
		None = 0,
		OK = 1,
		Yes = 2,
		No = 4,
		Cancel = 8,
		OKCancel = OK | Cancel,
		YesNo = Yes | No,
		YesNoCancel = Yes | No | Cancel
	}

	public class MessageBox
	{
		public delegate void MessageBoxResult (MessageBoxButton buttonPushed);

		public static void Show(string message)
		{
			Show (message, "Alert!", MessageBoxButton.OK, null);
		}

		public static void Show(string message, string title)
		{
			Show (message, title, MessageBoxButton.OK, null);
		}

		public static void Show(string message, string title, MessageBoxButton buttons)
		{
			Show (message, title, buttons, null);
		}

		public static void Show(string message, string title, MessageBoxResult resultCB)
		{
			Show (message, title, MessageBoxButton.OK, resultCB);
		}

		public static void Show(string message, string title, MessageBoxButton buttons, MessageBoxResult resultCB)
		{
			Button btnOK = new Button ();

			Window msgBox = new Window();
			msgBox.Title = title;

			Label lblMessage = new Label ();
			lblMessage.Font = new Font (Gui.Settings.DefaultFont, Gui.Settings.DefaultFontSize);
			//lblMessage.Size = new Vector2 (400, 1); // Label can be maximum of 400px wide
			lblMessage.MaxSize = new Vector2 (400, 1); // Label can be maximum of 400px wide
			lblMessage.Value = message;

			// TODO: Support for other buttons 

			Vector2 buttonsSize = new Vector2(0,30); // label and buttons has 20px space in between them, and button have 10px space to the bottom border of the window

			if((buttons & MessageBoxButton.OK) == MessageBoxButton.OK)
			{
				btnOK.Value = "OK";
				btnOK.GrabInput = true;

				btnOK.MouseClickEvent += (button) => {
					if(resultCB != null)
					{
						resultCB (MessageBoxButton.OK);
					}
					Gui.Remove(msgBox);
				};

				buttonsSize += btnOK.Size;

				msgBox.Children.AddChild (btnOK);
			}

			msgBox.Size = lblMessage.Size + buttonsSize;

			lblMessage.Position = new Vector2 (msgBox.Size.X / 2 - lblMessage.Size.X / 2, 5);

			if ((buttons & MessageBoxButton.OK) == MessageBoxButton.OK) 
			{
				btnOK.Position = new Vector2 (msgBox.Size.X / 2 - ((btnOK.Size.X + btnOK.Padding.X + btnOK.Padding.Z) / 2), msgBox.Size.Y - btnOK.Size.Y);
			}

			msgBox.Children.AddChild (lblMessage);


			Gui.Add (msgBox);
		}
	}
}

