using System;
using OpenTK;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.Models;
using OpenTK.Graphics.OpenGL;
using hgame1.Graphics.GUI.Controllers;
using hgame1.Utilities;
using System.Collections.Generic;

namespace hgame1.Graphics.GUI.Drawers
{
	public class ButtonDrawer : IGuiDrawer
	{
		protected ShaderProgram Shader, TitleShader;

		Model drawModel, textPlane;

		Vector4 hoverColor = new Vector4 (0.5f, 0.5f, 0.5f, 1f);
		Vector4 backgroundColor = new Vector4 (0.3f, 0.3f, 0.3f, 1f);
		Vector4 borderColor = new Vector4(0.1f, 0.1f, 0.1f, 1f);

		protected ButtonDrawerSettings Settings;

		public ButtonDrawer ()
		{
		}

		#region IGuiDrawer implementation

		public void Initialize (GuiDrawerSettings settings)
		{
			Settings = (ButtonDrawerSettings)settings;

			Vector4 color = Gui.Settings.Color;

			if(Settings != null)
			{
				hoverColor = Settings.HoverColor;
				backgroundColor = Settings.BackgroundColor;
				borderColor = Settings.BorderColor;
			}

			// Get drawing plane
			textPlane = ModelManager.Get ("Gui.TextureDrawer.DrawPlane");

			// If getting the drawing plane failed, create one
			if(textPlane == null)
			{
				textPlane = Primitives.Plane (0, 0, 1, 1);
				textPlane.Initialize ();
				ModelManager.Add ("Gui.TextureDrawer.DrawPlane", textPlane);
			}

			// Get colors
			VectorHelper.Multiply(ref hoverColor, ref color, out hoverColor);
			VectorHelper.Multiply(ref backgroundColor, ref color, out backgroundColor);

			// Get shaders configured in gui.xml
			Shader = ShaderProgramManager.Get ("gui.button");
			TitleShader = ShaderProgramManager.Get ("gui.texture");

			if (Shader == null)
				throw new ApplicationException ("Shader gui.button must be configured in gui.xml!");

			if(TitleShader == null)
				throw new ApplicationException ("Shader gui.texture must be configured in gui.xml!");
		}

		void RenderTitle(Vector2 size, Vector2 pos)
		{
			// Create model matrix for drawing
			Matrix4 modelMatrix = Matrix4.Scale (size.X, size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (pos));

			// Send the model matrix to the shader
			TitleShader.SendUniform ("mM", ref modelMatrix);

			// Draw the plane
			textPlane.Meshes[0].Render ();
		}

		Dictionary<Button, Vector2> sizes = new Dictionary<Button, Vector2>();
		Dictionary<Button, Model> drawModels = new Dictionary<Button, Model>();

		public void Draw (GuiController obj)
		{
			// Cast to label object
			Button button = (Button)obj;

			if(sizes.ContainsKey(button))
			{
				if(sizes[button] != button.Size)
				{
					drawModels[button] = Primitives.RoundBox(button.Size.X - Settings.CornerRoundness * 2, button.Size.Y - Settings.CornerRoundness * 2, Settings.CornerRoundness, Settings.CornerSlices);
					drawModels[button].Initialize ();
				}
			}
			else
			{
				sizes.Add (button, new Vector2 ());
				drawModels.Add(button, Primitives.RoundBox(button.Size.X - Settings.CornerRoundness * 2, button.Size.Y - Settings.CornerRoundness * 2, Settings.CornerRoundness, Settings.CornerSlices));
				drawModels[button].Initialize ();
			}

			// Store the button size for next round
			sizes [button] = button.Size;

			// Get the draw model
			drawModel = drawModels [button];



			//button.ChildrenOffset = new Vector2(button.Padding.X, button.Padding.Y);

			Vector2 offset = Vector2.Zero;

			if(button.Parent != null)
			{
				offset += button.Parent.Position + button.Parent.ChildrenOffset;
			}

			Vector4 color = backgroundColor;
			float fade = Settings.FadeAmount;

			if(button.Hovered)
			{
				color = hoverColor;
			}

			Shader.Enable ();

			// Send the model matrix to the shader
			Shader.SendUniform ("mP", ref Gui.GuiProjection);

			Matrix4 modelMatrix = Matrix4.CreateRotationX((float)(Math.PI)) * Matrix4.CreateTranslation (new Vector3 (button.Position + (button.Size / 2) + offset));

			//Console.WriteLine ("Position: {0}\nSize: {1}\nModelMatrix:\n{2}", button.Position, button.Size, modelMatrix );

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Send the color to the shader
			Shader.SendUniform ("color", ref color);

			// Send the color to the shader
			Shader.SendUniform ("fade", fade);

			// Render the button graphics
			drawModel.Meshes [0].Render ();

			Shader.Disable ();

			TitleShader.Enable ();

			// Send the model matrix to the shader
			TitleShader.SendUniform ("mP", ref Gui.GuiProjection);

			// If there is no prerendered texture, do nothing
			if (button.Texture == null)
				return;

			// Bind the texture
			button.Texture.Bind ();

			Vector2 pos = offset + button.Position + (button.Size / 2) - button.Texture.Size / 2.0f;
			Vector2 size = button.Texture.Size;

			RenderTitle (size, pos);

			// UnBind the texture
			button.Texture.UnBind ();
		}

		#endregion
	}
}

