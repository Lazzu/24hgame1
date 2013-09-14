using System;
using hgame1.Graphics;

namespace hgame1
{
	public class Settings
	{
		public GraphicsSettings Graphics = new GraphicsSettings();

		string resourcePath;
		public string ResourcePath {
			get {
				return resourcePath + "/";
			}
			set {
				resourcePath = value;
			}
		}

		string texturePath;
		public string TexturePath {
			get {
				return resourcePath + "/" + texturePath + "/";
			}
			set {
				texturePath = value;
			}
		}

		string shaderPath;
		public string ShaderPath {
			get {
				return resourcePath + "/" + shaderPath + "/";
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

		public static Settings CurrentSettings = new Settings();
	}
}

