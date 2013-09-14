using System;
using hgame1.Graphics.GUI.Controllers;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.Models;
using OpenTK;
using hgame1.Graphics.GUI.Controllers.Windows;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.GUI.Drawers
{
	public class WindowDrawer : IGuiDrawer
	{
		protected ShaderProgram Shader, TitleShader;

		Model drawPlane;

		int hVbo, vVbo, horizontal, vertical, hEbo, vEbo;

		Vector2[] horLine = new Vector2[2]{
			new Vector2(0,0),
			new Vector2(1,0)
		};

		Vector2[] verLine = new Vector2[2]{
			new Vector2(0,0),
			new Vector2(0,1)
		};

		uint[] index = new uint[2]{
			0,1
		};

		Vector4 bordercolor = new Vector4 (0, 0.5f, 0, 0.75f);
		Vector4 backgroundColor = new Vector4 (0, 0.1f, 0, 0.75f);

		public WindowDrawer ()
		{
		}

		#region IGuiDrawer implementation

		public void Initialize (GuiDrawerSettings settings)
		{
			// Get drawing plane
			drawPlane = ModelManager.Get ("Gui.TextureDrawer.DrawPlane");

			// If getting the drawing plane failed, create one
			if(drawPlane == null)
			{
				drawPlane = Primitives.Plane (0, 0, 1, 1);
				drawPlane.Initialize ();
				ModelManager.Add ("Gui.TextureDrawer.DrawPlane", drawPlane);
			}

			Shader = new ShaderProgram ();
			Shader.ProcessShaderFile ("gui/draw.vert", ShaderType.VertexShader);
			Shader.ProcessShaderFile ("gui/colored.frag", ShaderType.FragmentShader);
			Shader.Link ();

			TitleShader = new ShaderProgram ();
			TitleShader.ProcessShaderFile ("gui/draw.vert", ShaderType.VertexShader);
			TitleShader.ProcessShaderFile ("gui/draw.frag", ShaderType.FragmentShader);
			TitleShader.Link ();

			// Find matrix uniforms
			Shader.FindUniforms (new string[] {
				"mP", "mM", "color"
			});

			// Find matrix uniforms
			TitleShader.FindUniforms (new string[] {
				"mP", "mM"
			});

			GL.GenVertexArrays(1, out vertical);
			GL.GenVertexArrays(1, out horizontal);
			GL.GenBuffers(1, out vVbo);
			GL.GenBuffers(1, out vEbo);
			GL.GenBuffers(1, out hVbo);
			GL.GenBuffers(1, out hEbo);

			// bind the vao
			GL.BindVertexArray(vertical);

			// Vertex buffer object (mesh vertex data)
			GL.BindBuffer(BufferTarget.ArrayBuffer, vVbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(verLine.Length * Vector2.SizeInBytes), verLine, BufferUsageHint.StaticDraw);

			// Set the vbo settings in the vao
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			// Element array object (indices)
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, vEbo);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(index.Length * sizeof(uint)), index, BufferUsageHint.StaticDraw);

			// Unbind the vao
			GL.BindVertexArray(0);

			// bind the vao
			GL.BindVertexArray(horizontal);

			// Vertex buffer object (mesh vertex data)
			GL.BindBuffer(BufferTarget.ArrayBuffer, hVbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(horLine.Length * Vector2.SizeInBytes), horLine, BufferUsageHint.StaticDraw);

			// Set the vbo settings in the vao
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

			// Element array object (indices)
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, hEbo);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(index.Length * sizeof(uint)), index, BufferUsageHint.StaticDraw);

			// Unbind the vao
			GL.BindVertexArray(0);
		}

		void RenderLine(int vao, Vector2 pos, Vector2 size, Vector4 color)
		{
			Matrix4 modelMatrix = 
				Matrix4.Scale (size.X, size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (pos));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Send the color to the shader
			Shader.SendUniform ("color", ref color);

			// bind the vao
			GL.BindVertexArray(vao);

			// Draw it
			GL.DrawRangeElements(BeginMode.Lines, 0, index.Length-1, index.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

			// unbind the vao
			GL.BindVertexArray(0);
		}

		void RenderBackground (Vector2 pos, Vector2 size, Vector4 color)
		{
			Matrix4 modelMatrix = 
				Matrix4.Scale (size.X, size.Y, 0) *
					Matrix4.CreateTranslation (new Vector3 (pos));

			// Send the model matrix to the shader
			Shader.SendUniform ("mM", ref modelMatrix);

			// Send the color to the shader
			Shader.SendUniform ("color", ref color);

			// Background
			drawPlane.Meshes[0].Render ();
		}

		void RenderTitle(Vector2 size, Vector2 pos)
		{
			// Create model matrix for drawing
			Matrix4 modelMatrix = Matrix4.Scale (size.X, size.Y, 0) *
				Matrix4.CreateTranslation (new Vector3 (pos));

			// Send the model matrix to the shader
			TitleShader.SendUniform ("mM", ref modelMatrix);

			// Draw the plane
			drawPlane.Meshes[0].Render ();
		}

		public void Draw (GuiController obj)
		{
			// Cast to label object
			Window window = (Window)obj;

			window.ChildrenOffset = new Vector2(window.BorderWidth, window.TitleHeight + (window.BorderWidth * 2) + (-window.Value.Y));

			Vector2 winpos = window.Position;
			Vector2 contentpos = window.Position + new Vector2 (window.BorderWidth, (window.BorderWidth * 2) + window.TitleHeight);
			Vector2 contentsize = window.Size - new Vector2 (window.BorderWidth * 2, (window.BorderWidth * 2) + window.TitleHeight);
			Vector2 titlepos = winpos + new Vector2 (window.BorderWidth, window.BorderWidth);
			Vector2 titlesize = new Vector2 (window.Size.X - window.BorderWidth * 2, window.TitleHeight);
			Vector2 vlinesize = new Vector2 (window.Size.X - window.BorderWidth * 2, window.BorderWidth);
			Vector2 hlinesize = new Vector2 (window.BorderWidth, window.Size.Y);
			Vector2 leftlinepos = winpos;
			Vector2 rightlinepos = winpos + new Vector2 (window.Size.X - window.BorderWidth, 0);
			Vector2 toplinepos = winpos + new Vector2 (window.BorderWidth, 0);
			Vector2 titlelinepos = toplinepos + new Vector2 (0, window.BorderWidth + window.TitleHeight);
			Vector2 bottomlinepos = winpos + new Vector2 (window.BorderWidth,  window.Size.Y - window.BorderWidth);

			Shader.Enable ();

			// Send the model matrix to the shader
			Shader.SendUniform ("mP", ref Gui.GuiProjection);

			// Render the background
			RenderBackground (contentpos , contentsize, backgroundColor);

			// Render the title background
			RenderBackground (titlepos, titlesize, bordercolor);

			// Top line
			RenderBackground (toplinepos, vlinesize, bordercolor);

			// Title line
			RenderBackground (titlelinepos, vlinesize, bordercolor);

			// Bottom line
			RenderBackground (bottomlinepos, vlinesize, bordercolor);

			// Left line
			RenderBackground (leftlinepos, hlinesize, bordercolor);

			// Right line
			RenderBackground (rightlinepos, hlinesize, bordercolor);

			Shader.Disable ();

			TitleShader.Enable ();

			// Send the model matrix to the shader
			TitleShader.SendUniform ("mP", ref Gui.GuiProjection);

			// If there is no prerendered texture, do nothing
			if (window.TitleTexture == null)
				return;

			// Bind the texture
			window.TitleTexture.Bind ();

			Vector2 pos = window.Position + new Vector2(window.BorderWidth, window.BorderWidth) + (new Vector2(window.Size.X / 2, window.TitleHeight / 2) - window.TitleTexture.Size / 2.0f);
			Vector2 size = window.TitleTexture.Size;

			RenderTitle (size, pos);

			// UnBind the texture
			window.TitleTexture.UnBind ();

			// Now there should be all the window's children being drawn.
			//GL.Scissor((int)(window.ChildrenOffset.X + window.Position.X), (int)(window.ChildrenOffset.Y + window.Position.Y), (int)window.Size.X, (int)window.Size.Y);
			//GL.Scissor((int)window.ChildrenOffset.X, (int)window.ChildrenOffset.Y, (int)window.Size.X, (int)window.Size.Y);
		}

		#endregion
	}
}

