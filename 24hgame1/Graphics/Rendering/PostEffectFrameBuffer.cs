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

		/*public PostEffectFrameBuffer (GameWindow gw)
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

			// Attach the depth buffer to the framebuffer
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, 
			                           FramebufferAttachment.DepthAttachment, 
			                           RenderbufferTarget.Renderbuffer, 
			                           depthBuffer);

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
			LightningEngine.SetupShader (shader);

			// uniform locations for debug rendering
			debugOrthoLocation = GL.GetUniformLocation(debugShader.Program, "mP");
			debugTextureLocation = GL.GetUniformLocation (debugShader.Program, "textureSampler");
		}

		void ResizeTextures(Point size)
		{
			/*if (oldschool)
				size = new Point (size.X / 2, size.Y / 2);*/

			// Create empty textures
			/*for(int i=0; i<textures.Length; i++)
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

			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
			GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, 
			                       RenderbufferStorage.DepthComponent32,
			                       size.X, size.Y);

			GL.Viewport(0,0,size.X,size.Y);
		}
*/
	}
}

