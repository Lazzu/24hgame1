using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.Shaders
{
	public class ShaderProgram : IDisposable
	{
		int program = 0;

		public int Program {
			get {
				return program;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this shader program is enabled as current.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled {
			get {
				return InternalShaderHelper.CurrentBoundShaderProgram == program;
			}
		}

		// List of shaders to prevent premature shader disposing.
		List<Shader> shaders = new List<Shader> ();

		// Uniform locations
		Dictionary<string, int> uniforms = new Dictionary<string, int>();

		// Uniform buffer objects
		Dictionary<string, int> uniformBuffers = new Dictionary<string, int>();

		public ShaderProgram ()
		{
			program = GL.CreateProgram();
		}

		public ShaderProgram (string name) : this()
		{
			ShaderProgramManager.Add (name, this);
		}

		public void FindUniforms(string[] uniform)
		{
			foreach(string u in uniform)
			{
				FindUniform (u);
			}
		}

		public void FindUniform(string uniform)
		{
			uniforms[uniform] = GL.GetUniformLocation(program, uniform);
		}

		public void CreateUniformBuffer(string uniform, int size, BufferUsageHint hint, int bufferIndex = 0)
		{
			int ubo;
			GL.GenBuffers (1, out ubo);
			uniformBuffers [uniform] = ubo;
			GL.BindBuffer (BufferTarget.UniformBuffer, ubo);
			GL.BufferData (BufferTarget.UniformBuffer, new IntPtr(size), IntPtr.Zero, hint);

			GL.BindBufferRange (BufferTarget.UniformBuffer, bufferIndex, ubo, IntPtr.Zero, new IntPtr(size));

			uniforms[uniform] = GL.GetUniformBlockIndex(program, uniform);
			GL.UniformBlockBinding (program, uniforms[uniform], bufferIndex);
		}

		public void Attach(Shader shader)
		{
			GL.AttachShader (program, shader.ShaderID);
			shaders.Add (shader);
		}

		public void ProcessShaderFile(string path, ShaderType type)
		{
			Shader shader = ShaderManager.FromFile (path, type);
			Attach (shader);
		}

		public void ProcessShader(string shaderSource, ShaderType type)
		{
			Shader shader = ShaderManager.FromSource (shaderSource, type);
			Attach (shader);
		}

		public void Link()
		{
			GL.LinkProgram(program);
			string programInfoLog;
			GL.GetProgramInfoLog( program, out programInfoLog );
			Console.WriteLine( programInfoLog );
		}

		public void Enable()
		{
			GL.UseProgram(program);
			InternalShaderHelper.CurrentBoundShaderProgram = program;
		}

		public void Disable()
		{
			GL.UseProgram(0);
			InternalShaderHelper.CurrentBoundShaderProgram = 0;
		}

		#region SendUniform methods
		public void SendUniform(string uniform, double data)
		{
			GL.Uniform1(uniforms[uniform], data);
		}
		public void SendUniform(string uniform, float data)
		{
			GL.Uniform1(uniforms[uniform], data);
		}
		public void SendUniform(string uniform, int data)
		{
			GL.Uniform1(uniforms[uniform], data);
		}
		public void SendUniform(string uniform, uint data)
		{
			GL.Uniform1(uniforms[uniform], data);
		}
		public void SendUniform(string uniform, short data)
		{
			GL.Uniform1(uniforms[uniform], data);
		}
		public void SendUniform(string uniform, byte data)
		{
			GL.Uniform1(uniforms[uniform], data);
		}
		public void SendUniform(string uniform, ref Vector2 data)
		{
			GL.Uniform2(uniforms[uniform], ref data);
		}
		public void SendUniform(string uniform, ref Vector3 data)
		{
			GL.Uniform3(uniforms[uniform], ref data);
		}
		public void SendUniform(string uniform, ref Vector4 data)
		{
			GL.Uniform4(uniforms[uniform], ref data);
		}
		public void SendUniform(string uniform, ref Matrix4 data)
		{
			GL.UniformMatrix4(uniforms[uniform], false, ref data);
		}
		public void SendUniform(string uniform, bool normalize, ref Matrix4 data)
		{
			GL.UniformMatrix4(uniforms[uniform], normalize, ref data);
		}
		public void SendUniformBlock<T> (string uniform, int size, T[] data, BufferUsageHint hint) where T : struct
		{
			GL.BindBuffer(BufferTarget.UniformBuffer, uniformBuffers[uniform]);
			GL.BufferData (BufferTarget.UniformBuffer, new IntPtr(size), data, hint);
			GL.BindBuffer(BufferTarget.UniformBuffer, 0);
		}
		// TODO: Implement all GL uniform methods
		#endregion

		#region IDisposable implementation
		bool disposed = false;
		public void Dispose (bool manual)
		{
			if (!disposed)
			{
				if (manual)
				{
					Console.WriteLine ("Disposing Shader Program {0}", program);
					shaders.Clear ();
					GL.DeleteProgram (program);
					GC.SuppressFinalize(this);
					disposed = true;
				}
				else
				{
					Console.WriteLine ("WARNING: Shader Program {0} not disposed manually!", program);
					GC.KeepAlive(ResourceDisposer.DisposingQueue);
					ResourceDisposer.DisposingQueue.Enqueue(this);
				}
			}
		}
		public void Dispose()
		{
			Dispose (true);
		}
		~ShaderProgram()
		{
			Dispose (false);
		}
		#endregion
	}
}

