using System;
using OpenTK;
using hgame1.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace hgame1.Graphics.Rendering
{
	public class PostEffectFrameBuffer
	{
		int fbo = 0;

		int[] textures = new int[3];
		string[] textureLocations = new string[]{
			"RT0","RT1","RT2"//,"RT3"
		};
		int debugTextureLocation;

		Matrix4 orthoMatrix;

		int debugOrthoLocation;

		bool oldschool = false;
		public bool Oldschool {
			get {
				return oldschool;
			}
			set {
				oldschool = value;
			}
		}

		Vector3 brightness = Vector3.One;
		public Vector3 Brightness {
			get {
				return brightness;
			}
			set {
				brightness = value;
			}
		}

		Vector3[] vertex = new Vector3[4]{
			new Vector3(0,0,0),
			new Vector3(1,0,0),
			new Vector3(0,1,0),
			new Vector3(1,1,0)
		};

		ushort[] index = new ushort[4]{ 0,1,2,3 };

		int vbo, ebo, vao;

		ShaderProgram debugShader;
		ShaderProgram shader;

		Vector2 WindowSize = Vector2.Zero;

		public PostEffectFrameBuffer (GameWindow gw)
		{
			Point size = new Point(gw.Size);

			GL.GenFramebuffers(1, out fbo);
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);

			// Generate textures to render to
			GL.GenTextures(textures.Length, textures);

			// Create new textures and buffers
			ResizeTextures (size);

			// Create orthogonal projection
			ResizeOrtho (size);

			// Draw buffer types list
			DrawBuffersEnum[] list = new DrawBuffersEnum[textures.Length];

			// Attach textures to the framebuffer and set the type in the list
			for (int i = 0; i < textures.Length; i++) {
				GL.FramebufferTexture(FramebufferTarget.Framebuffer,FramebufferAttachment.ColorAttachment0+i, textures[i], 0);
				list [i] = DrawBuffersEnum.ColorAttachment0 + i;
			}

			// Set the list of draw buffers.
			GL.DrawBuffers(textures.Length, list);

			// Get error code
			FramebufferErrorCode status = GL.CheckFramebufferStatus (FramebufferTarget.Framebuffer);

			// Check for errors
			if(status != FramebufferErrorCode.FramebufferComplete)
				throw new Exception("Error creating frame buffer! Status: " + status);

			// Unbind the framebuffer
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);



			// Vertex buffer object
			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(vertex.Length * Vector3.SizeInBytes), vertex, BufferUsageHint.DynamicDraw);

			// Element array object (indices)
			GL.GenBuffers(1, out ebo);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.BufferData (BufferTarget.ElementArrayBuffer, (IntPtr)(index.Length * sizeof(ushort)), index, BufferUsageHint.StaticDraw);

			// Vertex array object (store vertex buffer and incides in same place)
			GL.GenVertexArrays(1, out vao);

			// bind the vao
			GL.BindVertexArray(vao);

			// Set the vbo settings in the vao
			GL.EnableVertexAttribArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);

			// Bind the ebo to the vao
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);

			// Unbind the vao
			GL.BindVertexArray(0);

			ResizeVertices (size);


			// Make the shader
			shader = new ShaderProgram();

			shader.ProcessShaderFile ("shaders/deferred.vert", ShaderType.VertexShader);
			shader.ProcessShaderFile ("shaders/deferred.frag", ShaderType.FragmentShader);

			shader.Link ();


			// Make the debug shader
			debugShader = new ShaderProgram();
			debugShader.ProcessShaderFile("shaders/texture.vert", ShaderType.VertexShader);
			debugShader.ProcessShaderFile("shaders/texture.frag", ShaderType.FragmentShader);

			Console.WriteLine ("Link debug shader.");
			debugShader.Link ();

			// Matrix locations for deferred rendering
			shader.FindUniform ("mP");
			shader.FindUniform ("mIP");
			shader.FindUniform ("mV");
			shader.FindUniform ("mN");

			// Texture locations for deferred rendering
			shader.FindUniforms (textureLocations);

			// Camera position uniform
			shader.FindUniform ("cameraPosition");

			// Screen brightness
			shader.FindUniform ("brightness");

			// The light
			shader.FindUniform ("LightCount");

			// Screen size
			shader.FindUniform ("ScreenSize");

			// Setup lightning
			//LightningEngine.SetupShader (shader);

			// uniform locations for debug rendering
			debugOrthoLocation = GL.GetUniformLocation(debugShader.Program, "mP");
			debugTextureLocation = GL.GetUniformLocation (debugShader.Program, "textureSampler");
		}

		void ResizeTextures(Point size)
		{
			/*if (oldschool)
				size = new Point (size.X / 2, size.Y / 2);*/

			// Create empty textures
			for(int i=0; i<textures.Length; i++)
			{
				// Bind the texture
				GL.BindTexture(TextureTarget.Texture2D, textures[i]);

				// Give an empty image to OpenGL
				GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, size.X, size.Y, 0, 
				              PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

				// Poor filtering. Needed !
				GL.TexParameter(TextureTarget.Texture2D, 
				                TextureParameterName.TextureMinFilter, 
				                (int)TextureMagFilter.Nearest);
				GL.TexParameter(TextureTarget.Texture2D, 
				                TextureParameterName.TextureMagFilter, 
				                (int)TextureMagFilter.Nearest);
			}

			GL.Viewport(0,0,size.X,size.Y);
		}

		void ResizeOrtho(Point size)
		{
			Matrix4.CreateOrthographicOffCenter (0, size.X, 0, size.Y, 0, 1, out orthoMatrix);
		}

		void ResizeVertices (Point size)
		{
			vertex = new Vector3[4]{
				new Vector3(0,0,0),
				new Vector3(1,0,0),
				new Vector3(0,1,0),
				new Vector3(1,1,0)
			}; 

			// Scale the vector data to the screen size
			for (int i = 0; i < 4; i++) {
				vertex [i].X = vertex [i].X * size.X;
				vertex [i].Y = vertex [i].Y * size.Y;
			}

			// Send new data
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(vertex.Length * Vector3.SizeInBytes), vertex, BufferUsageHint.DynamicDraw);
			GL.BindBuffer (BufferTarget.ArrayBuffer, 0);

			WindowSize = new Vector2 (size.X, size.Y);
		}

		public void Resize(Point size)
		{
			ResizeTextures (size);
			ResizeOrtho (size);
			ResizeVertices (size);
		}

		public void RenderOnScreen ()
		{
			// Disable depth test to allow drawing on top of everything
			GL.Disable (EnableCap.DepthTest);

			// Use the shader
			shader.Enable ();

			// Send lights
			//shader.SendUniformBlock ("Light", LightningEngine.Buffer.Length * Light.SizeInBytes, LightningEngine.Buffer, BufferUsageHint.StreamDraw);

			// Bind textures
			/*for(int i=0; i<textures.Length; i++)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + i);
				GL.BindTexture (TextureTarget.Texture2D,textures [i]);
				shader.SendUniform (textureLocations [i], i);
			}


			GL.ActiveTexture(TextureUnit.Texture3);
			GL.BindTexture (TextureTarget.Texture2D,textures [3]);
			GL.Uniform1 (textureLocations[3], 3);*/

			// Bind the VAO to be drawn
			GL.BindVertexArray(vao);

			// Set the View and Projection matrices
			shader.SendUniform ("mP", ref orthoMatrix);
			//shader.SendUniform ("mIP", ref Camera.InvProjectionMatrix);
			shader.SendUniform ("mV", ref Camera.ViewMatrix);
			shader.SendUniform ("mN", ref Camera.NormalMatrix);

			// Send current screen size
			shader.SendUniform ("ScreenSize", ref WindowSize);

			// Send the camera position
			//shader.SendUniform ("cameraPosition", ref Camera.Position);

			shader.SendUniform ("brightness", ref brightness);
			//shader.SendUniform ("LightCount", LightningEngine.LightsCount);

			//foreach (var light in LightningEngine.Lights) {

				//shader.SendUniform ("Light", light.ToFloatArray());



				// Draw quad
				//GL.DrawRangeElements(BeginMode.TriangleStrip, 0, index.Length-1, index.Length, DrawElementsType.UnsignedShort, IntPtr.Zero);
			//}

			GL.DrawRangeElements(BeginMode.TriangleStrip, 0, index.Length-1, index.Length, DrawElementsType.UnsignedShort, IntPtr.Zero);

			// Unbind VAO
			GL.BindVertexArray(0);

			// Disable shader
			shader.Disable ();

			// Unbind textures
			for(int i=0; i<textures.Length; i++)
			{
				GL.ActiveTexture(TextureUnit.Texture0 + i);
				GL.BindTexture (TextureTarget.Texture2D,0);
			}

			// Re-enable depth test
			GL.Enable (EnableCap.DepthTest);
		}

		/// <summary>
		/// Renders all textures as debug data on screen.
		/// </summary>
		public void RenderDebugOnScreen ()
		{
			// Disable depth test to allow drawing on top of everything
			GL.Disable (EnableCap.DepthTest);

			// Bind the VAO to be drawn
			GL.BindVertexArray(vao);

			// Use the shader
			debugShader.Enable ();

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Uniform1 (debugTextureLocation, 0);

			for(int i = 0; i < textures.Length; i++) {

				// Bind the texture for the shader to use
				GL.BindTexture (TextureTarget.Texture2D,textures [i]);

				float left = -i;
				float right = textures.Length-i;
				// Matrix for positioning the texture on screen
				Matrix4 tempMatrix = Matrix4.CreateOrthographicOffCenter (left, right, 0, textures.Length, 0, 1);

				// Set the View and Projection matrices
				GL.UniformMatrix4(debugOrthoLocation, false, ref tempMatrix);

				// Draw quad
				GL.DrawRangeElements(BeginMode.TriangleStrip, 0, index.Length-1, index.Length, DrawElementsType.UnsignedShort, IntPtr.Zero);

			}

			// Disable shader
			debugShader.Disable ();

			// Unbind VAO
			GL.BindVertexArray(0);

			// Re-enable depth test
			GL.Enable (EnableCap.DepthTest);
		}

		public void StartRender()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
		}

		public void StopRender()
		{
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

			//RenderLights ();
		}

	}
}

