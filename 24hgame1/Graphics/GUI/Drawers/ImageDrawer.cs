using System;
using OpenTK;
using hgame1.Graphics.GUI.Controllers;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.Models;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.GUI.Drawers
{
	public class ImageDrawer : IGuiDrawer
	{
		protected ShaderProgram Shader;
		protected Model DrawPlane;

		public ImageDrawer ()
		{
			// Get drawing plane
			DrawPlane = ModelManager.Get ("Gui.TextureDrawer.DrawPlane");

			// If getting the drawing plane failed, create one
			if(DrawPlane == null)
			{
				DrawPlane = Primitives.Plane (0, 0, 1, 1);
				DrawPlane.Initialize ();
				ModelManager.Add ("Gui.TextureDrawer.DrawPlane", DrawPlane);
			}
		}

		public void Initialize(GuiDrawerSettings settings)
		{
			Shader = new ShaderProgram ();
			Shader.ProcessShaderFile ("gui/draw.vert", ShaderType.VertexShader);
			Shader.ProcessShaderFile ("gui/draw.frag", ShaderType.FragmentShader);
			Shader.Link ();

			// Find matrix uniforms
			Shader.FindUniforms (new string[] {
				"mP", "mM"
			});
		}

		public void Draw (GuiController obj)
		{
			// Cast to label object
			Image img = (Image)obj;

			// If there is no prerendered texture, do nothing
			if (img.Texture == null)
				return;

			// Create model matrix for drawing
			Matrix4 modelMatrix = 
				Matrix4.Scale (obj.Size.X, obj.Size.Y, 0) *
					Matrix4.CreateTranslation (new Vector3 (obj.Position));

			Shader.Enable ();

			// Send the model matrix to the shader
			Shader.SendUniform ("mP", ref Gui.GuiProjection);

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			img.Texture.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Unbind texture
			img.Texture.UnBind ();

			Shader.Disable ();
		}

	}
}

