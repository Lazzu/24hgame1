using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.Textures
{
	public class Texture : IDisposable
	{
		/// <summary>
		/// Gets or sets the path of the file the texture is located in.
		/// </summary>
		/// <value>The path of the file.</value>
		public string FileName {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the identifying name of the texture.
		/// </summary>
		/// <value>The identifying name of the texture.</value>
		public string Name {
			get;
			set;
		}

		protected int TextureID;
		/// <summary>
		/// Gets or sets the GL texture name.
		/// </summary>
		/// <value>The GL texture name.</value>
		public int GLTexture {
			get {
				return TextureID;
			}
			set {
				TextureID = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the texture.
		/// </summary>
		/// <value>The size of the texture.</value>
		public Vector2 Size {
			get;
			set;
		}

		/// <summary>
		/// Gets the width of the texture.
		/// </summary>
		/// <value>The width of the texture.</value>
		public float Width {
			get { return Size.X; }
		}

		/// <summary>
		/// Gets the height of the texture.
		/// </summary>
		/// <value>The height of the texture.</value>
		public float Height {
			get { return Size.Y; }
		}

		protected TextureTarget TextureTarget = TextureTarget.Texture2D;
		public TextureUnit TextureUnit = TextureUnit.Texture0;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasoloikka.Engine.Graphics.Texture"/> class.
		/// </summary>
		public Texture ()
		{
			Size = Vector2.Zero;
			TextureID = GL.GenTexture();
			Name = "";
			FileName = "";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tasoloikka.Engine.Graphics.Texture"/> class.
		/// </summary>
		/// <param name="glId">GL texture name.</param>
		/// <param name="fileName">File path.</param>
		/// <param name="name">Identifying name of the texture.</param>
		/// <param name="size">Size of the texture.</param>
		public Texture (int glId, string fileName, string name, Vector2 size)
		{
			this.GLTexture = glId;
			this.FileName = fileName;
			this.Name = name;
			this.Size = size;
		}

		/// <summary>
		/// Bind the texture for usage.
		/// </summary>
		public virtual void Bind()
		{
			GL.ActiveTexture(TextureUnit);
			GL.BindTexture (TextureTarget, TextureID);
		}

		/// <summary>
		/// Unbind the texture.
		/// </summary>
		public virtual void UnBind()
		{
			GL.ActiveTexture(TextureUnit);
			GL.BindTexture(TextureTarget, 0);
		}

		#region IDisposable
		// Implement IDisposable
		public bool Disposed {
			get {
				return TextureDisposed;
			}
		}
		protected bool TextureDisposed = false;
		public void Dispose()
		{
			Dispose(true);
		}
		protected virtual void Dispose (bool manual)
		{
			if (!TextureDisposed) {

				if (manual) 
				{
					if(GLTexture != -1) GL.DeleteTexture (TextureID);
					TextureDisposed = true;
					GC.SuppressFinalize(this);
				}
				else
				{
					GC.KeepAlive(ResourceDisposer.DisposingQueue);
					ResourceDisposer.DisposingQueue.Enqueue(this);
					if(Name != "") // If there is no name, it propably is generated on fly
						Console.WriteLine("Warning: Texture '"+Name+"' not unloaded manually!");
				}


			}
		}
		~Texture()
		{
			Dispose (false);
		}
		#endregion
	}
}

