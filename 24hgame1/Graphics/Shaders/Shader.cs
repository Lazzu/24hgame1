using System;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace hgame1.Graphics.Shaders
{
	public class Shader : IDisposable
	{
		int shader;
		public int ShaderID {
			get {
				return shader;
			}
		}

		public ShaderType Type {
			get;
			protected set;
		}

		public string Hash {
			get;
			private set;
		}

		public Shader (ShaderType type)
		{
			Type = type;
			shader = GL.CreateShader(type);
		}

		public void FromSource(string source)
		{
			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);
			Hash = hgame1.Utilities.Hash.GetHashString (source);
		}


		#region IDisposable implementation
		bool disposed = false;
		public void Dispose (bool manual)
		{
			if (!disposed)
			{
				if (manual)
				{
					GL.DeleteShader (shader);
					GC.SuppressFinalize(this);
					disposed = true;
				}
				else
				{
					Console.WriteLine ("WARNING: Shader {0} not disposed manually!", ShaderID);
					GC.KeepAlive(this);
					ResourceDisposer.DisposingQueue.Enqueue(this);
				}
			}
		}
		public void Dispose()
		{
			Dispose (true);
		}
		~Shader()
		{
			Dispose (false);
		}
		#endregion
	}
}

