using System;
using hgame1.Graphics.Models;
using hgame1.Graphics.GUI.Controllers;
using hgame1.Graphics.Shaders;
using OpenTK;
using hgame1.Graphics.Textures;
using hgame1.Graphics.GUI.Controllers.Windows;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.GUI.Drawers
{
	public class TexturedWindowDrawer : IGuiDrawer
	{
		protected ShaderProgram Shader;
		protected Model DrawPlane;

		protected Texture TitleBar;

		protected Texture TopLeft;
		protected Texture TopRight;

		protected Texture LeftBorder;
		protected Texture Rightborder;
		protected Texture BottomBorder;

		protected Texture BottomLeft;
		protected Texture BottomRight;

		protected Texture Background;

		public TexturedWindowDrawer ()
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
			TexturedWindowDrawerSettings s = (TexturedWindowDrawerSettings)settings;

			TitleBar = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/titlebar.png", "Gui.Window.TitleBar");
			TopLeft = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/topleft.png", "Gui.Window.TopLeft");
			TopRight = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/topright.png", "Gui.Window.TopRight");
			BottomLeft = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/bottomleft.png", "Gui.Window.BottomLeft");
			BottomRight = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/bottomright.png", "Gui.Window.BottomRight");
			LeftBorder = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/left.png", "Gui.Window.LeftBorder");
			Rightborder = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/right.png", "Gui.Window.RightBorder");
			BottomBorder = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/bottom.png", "Gui.Window.BottomBorder");
			Background = TextureManager.LoadTexture ("gui/" + s.Skin + "/window/winbg.png", "Gui.Window.Background");

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
			Window window = (Window)obj;
			Matrix4 modelMatrix;

			Shader.Enable ();

			// Send the model matrix to the shader
			Shader.SendUniform ("mP", ref Gui.GuiProjection);

			window.ChildrenOffset = TopLeft.Size;

			// Draw title bar

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (window.Size.X, TopLeft.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(TopLeft.Size.X, 0)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			TitleBar.Bind ();

			// Draw the plane
			DrawPlane.Meshes [0].Render ();

			// Draw top left corner

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (TopLeft.Size.X, TopLeft.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			TopLeft.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw top right corner

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (TopRight.Size.X, TopRight.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(TopLeft.Size.X + window.Size.X, 0)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			TopRight.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw left border

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (TopLeft.Size.X, window.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(0, TopLeft.Size.Y)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			LeftBorder.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw right border

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (TopRight.Size.X, window.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(TopLeft.Size.X + window.Size.X, TopLeft.Size.Y)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			Rightborder.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw bottom left corner

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (BottomLeft.Size.X, BottomLeft.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(0, TopLeft.Size.Y + window.Size.Y)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			BottomLeft.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw bottom right corner

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (BottomRight.Size.X, BottomRight.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(TopLeft.Size.X + window.Size.X, TopLeft.Size.Y + window.Size.Y)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			BottomRight.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw bottom border

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (window.Size.X, BottomRight.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(TopLeft.Size.X, TopLeft.Size.Y + window.Size.Y)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			BottomBorder.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw window background

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (window.Size.X, window.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + new Vector2(TopLeft.Size.X, TopLeft.Size.Y)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			Background.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Draw title text

			// If there is no prerendered texture, do nothing
			if (window.TitleTexture == null)
				return;

			// Create model matrix for drawing
			modelMatrix = Matrix4.Scale (window.TitleTexture.Size.X, window.TitleTexture.Size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (window.Position + (new Vector2(TopLeft.Size.X + window.Size.X / 2, TitleBar.Size.Y / 2) - window.TitleTexture.Size / 2.0f)));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Bind the texture
			window.TitleTexture.Bind ();

			// Draw the plane
			DrawPlane.Meshes[0].Render ();

			// Unbind texture
			window.TitleTexture.UnBind ();
		}
	}
}

