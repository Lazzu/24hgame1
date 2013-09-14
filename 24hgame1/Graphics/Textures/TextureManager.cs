using System;
using System.Collections.Concurrent;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace hgame1.Graphics.Textures
{
	public static class TextureManager
	{
		/// <summary>
		/// The textures dictionary.
		/// </summary>
		static ConcurrentDictionary<string, Texture> textures;

		/// <summary>
		/// Initializes a new instance of the <see cref="Engine.Graphics.Textures.TextureManager"/> class.
		/// </summary>
		/// <param name="gw">Gamewindow the texturemanager will bind itself onto.</param>
		public static void Init(OpenTK.Platform.IGameWindow gw)
		{
			// Initialize texture dictionary
			textures = new ConcurrentDictionary<string, Texture> ();

			// If textures need to be reloaded at any time
			gw.Load += ReloadTextures;
			gw.Closing += HandleClosing;
		}

		/// <summary>
		/// Adds the texture to the dictionary.
		/// </summary>
		/// <param name="t">The texture.</param>
		public static void AddTexture(Texture t)
		{
			// Add texture to the dictionary, using the texture name as index
			textures.TryAdd(t.Name, t);
		}

		/// <summary>
		/// Loads a texture from a file.
		/// </summary>
		/// <returns>The texture.</returns>
		/// <param name="file">Relative path of the texture file.</param>
		/// <param name="name">Identifying name of the texture.</param>
		public static Texture LoadTexture (string file, string name = null)
		{
			// TODO: Find out the correct path from settings or somewhere else
			string path = file;

			// See if at least the file exists
			if (String.IsNullOrEmpty (file))
				throw new ArgumentException (file);

			// See if name has been given or not, if not use the file name
			if (String.IsNullOrEmpty (name))
				name = file;

			// If the texture has already been loaded, return it
			if (textures.ContainsKey (name))
				return textures [name];

			// Check if file exists
			if (!File.Exists (path)) 
			{
				throw new FileNotFoundException ("Texture file not found!", file);
			}
			// Load image as bitmap
			Bitmap bm = new Bitmap (path);
			try {
				// Upload the bitmap to the GPU and return texture object
				return Load (bm, file, name);
			}
			finally {
				// If the load fails or not, dispose the bitmap in the end
				bm.Dispose ();
			}
		}

		// TODO: Add loading from stream to support archive files

		/// <summary>
		/// Unload the specified texture.
		/// </summary>
		/// <param name="name">Identifying name of the texture.</param>
		public static void Unload(string name)
		{
			textures [name].Dispose ();
			Texture temp;
			textures.TryRemove(name, out temp);
		}

		/// <summary>
		/// Unload all textures.
		/// </summary>
		public static void Unload()
		{
			foreach (KeyValuePair<string, Texture> item in textures) {
				//Console.WriteLine ("Unloading texture " + item.Key);
				item.Value.Dispose();
			}
			textures.Clear ();
		}

		/// <summary>
		/// Get the specified Texture.
		/// </summary>
		/// <param name="name">Identifying name of the texture.</param>
		public static Texture Get(string name)
		{
			if (textures.ContainsKey (name))
				return textures [name];

			return null;
		}

		/// <summary>
		/// Load a texture from specified bitmap, and saving file path and identifying name to the texture object.
		/// </summary>
		/// <param name="bitmap">Bitmap to load the texture from.</param>
		/// <param name="file">Relative file path to save to the texture object.</param>
		/// <param name="name">Identifying name of the texture.</param>
		public static Texture Load(Bitmap bitmap, string file, string name)
		{
			// Generate OpenGL Name for the texture
			int id = -1;
			GL.GenTextures(1, out id);

			// If texture generation failed, return null.
			if (id == -1) {
				Console.WriteLine("GL Texture generation failed!");
				return null;
			}

			// Get the size of the bitmap
			Rectangle size = new Rectangle (0, 0, bitmap.Width, bitmap.Height);

			// Create new Texture object
			Texture t = new Texture(){
				GLTexture = id, 
				FileName = file,
				Name = name,
				Size = new Vector2(bitmap.Width, bitmap.Height)
			};

			// Bind the texture object so it can be used (written in)
			t.Bind ();

			// Get the bitmap data
			BitmapData bitmapData = 
				bitmap.LockBits(
					size,
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb
					);

			// Use the bitmap as texture
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0,
			              OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);

			/*GL.TexSubImage2D (TextureTarget.Texture2D, 0, 0, 0, bitmapData.Width, bitmapData.Height, 
			                  OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, bitmapData.Scan0);*/

			// Release the bitmap
			bitmap.UnlockBits(bitmapData);

			// Generate mipmaps for the texture
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			// Set texture parameters
			//GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Modulate);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			// Unbind the texture
			t.UnBind ();

			// Add the texture to the dictionary
			AddTexture(t);

			// Return the texture
			return t;
		}

		/// <summary>
		/// Reloads the textures in the dictionary to the GPU memory in case they are lost.
		/// </summary>
		/// <param name="o">Object that called the event.</param>
		/// <param name="e">Event arguments.</param>
		static void ReloadTextures(object o, EventArgs e)
		{
			// If there are no textures, do nothing
			if(textures.Count <= 0) {
				return;
			}

			// Tell console that we are reloading
			Console.WriteLine ("Reloading all textures in TextureManager");

			// Make a temporary copy of the old texture dictionary
			Dictionary<string, Texture> tmp = new Dictionary<string, Texture>(textures);

			// Clear the old dictionary
			textures.Clear ();

			// Reload the textures using the values in the temporary dictionary
			foreach (KeyValuePair<string, Texture> item in tmp) {
				LoadTexture(item.Value.FileName, item.Value.Name);
			}

			tmp.Clear ();
		}

		static void HandleClosing (object sender, CancelEventArgs e)
		{
			if(textures.Count > 0)
			{
				//Console.WriteLine ("Unloading all textures.");
				Unload ();
			}
		}
	}
}

