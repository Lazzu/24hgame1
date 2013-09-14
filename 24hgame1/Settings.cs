using System;
using hgame1.Graphics;
using System.IO;

namespace hgame1
{
	public class Settings
	{
		public GraphicsSettings Graphics = new GraphicsSettings();

		string resourcePath;
		public string ResourcePath {
			get {
				return resourcePath;
			}
			set {
				resourcePath = value;
			}
		}

		string texturePath;
		public string TexturePath {
			get {
				return texturePath;
			}
			set {
				texturePath = value;
			}
		}

		string shaderPath;
		public string ShaderPath {
			get {
				return shaderPath;
			}
			set {
				shaderPath = value;
			}
		}

		public Settings ()
		{
			resourcePath = "Content";
			texturePath = "Textures";
			shaderPath = "Shaders";
		}

		public string GetTexturePath(string file)
		{
			return Path.Combine (resourcePath, Path.Combine (texturePath, file));
		}

		public string GetShaderPath(string file)
		{
			return Path.Combine (resourcePath, Path.Combine (shaderPath, file));
		}

		public static readonly Settings DefaultSettings = new Settings();
		public static Settings CurrentSettings = new Settings();
	}
}

