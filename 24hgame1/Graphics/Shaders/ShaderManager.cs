using System;
using System.IO;
using System.Collections.Generic;
using hgame1.Utilities;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.Shaders
{
	public static class ShaderManager
	{
		static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();

		static ShaderPreprocessor preProcessor = new ShaderPreprocessor();

		public static ShaderPreprocessor PreProcessor {
			get {
				return preProcessor;
			}
		}

		static bool initialized = false;

		public static void Initialize()
		{
			if (initialized)
				return;

			/*PreProcessor.Add(":znear:", Camera.ZNear.ToString());
			PreProcessor.Add(":zfar:", Camera.ZFar.ToString());
			PreProcessor.Add(":debug:", "on");
			PreProcessor.Add (":maxLights:", LightningEngine.HardLimit.ToString());*/
		}

		/// <summary>
		/// Creates a shader from a file, or returns previously created shader using the same file.
		/// </summary>
		/// <returns>The shader.</returns>
		/// <param name="file">Shader file.</param>
		/// <param name="type">Shader type.</param>
		public static Shader FromFile(string file, ShaderType type)
		{
			file = Settings.CurrentSettings.GetShaderPath (file);

			// Check if file path already exists in the dictionary
			if(shaders.ContainsKey(file))
			{
				// If shader type is a match
				if (shaders [file].Type == type)
					return shaders [file];
			}

			// Read the file
			using(FileStream filestream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using(StreamReader reader = new StreamReader(filestream, System.Text.Encoding.UTF8))
				{
					string source = reader.ReadToEnd ();

					// Create shader using the source code
					Shader shader = FromSource (source, type);

					// Add the shader to the dictionary using the file path as key
					shaders.Add (file, shader);

					return shader;
				}
			}
		}

		/// <summary>
		/// Creates a shader from source, or returns previously created shader by the same source.
		/// </summary>
		/// <returns>The shader.</returns>
		/// <param name="source">Shader source.</param>
		/// <param name="type">Shader type.</param>
		public static Shader FromSource(string source, ShaderType type)
		{
			// Initialize stuff, mainly preprocessor.
			Initialize ();

			// Preprocess the shader source
			source = preProcessor.Process (source);

			// Get hash of the shader source
			string hash = Hash.GetHashString (source + type.ToString());

			// If shader hash exists in the dictionary, return the shader
			if(shaders.ContainsKey(hash))
			{
				return shaders[hash];
			}

			// Create new shader with the type
			Shader shader = new Shader (type);

			// Give the shader source to the shader we just created
			shader.FromSource (source);

			// Add the created shader to the dictionary using the hash as a key
			shaders.Add (hash, shader);

			return shader;
		}

		/// <summary>
		/// Remove all instances of the specified shader from the manager.
		/// </summary>
		/// <param name="shader">Shader.</param>
		public static void Remove(Shader shader)
		{
			// List of shader keys that we should remove from the shaders dictionery
			List<string> keys = new List<string> ();

			// Loop through the shaders
			foreach (var kvp in shaders) {
				// If shader matches, add the key to the list
				if (kvp.Value == shader)
					keys.Add (kvp.Key);
			}

			// Remove all keys from the dictionary
			foreach (var key in keys) {
				shaders.Remove (key);
			}
		}

		/// <summary>
		/// Remove all shaders that are the same as the shader found with file name or hash.
		/// </summary>
		/// <param name="fileOrHash">File or hash.</param>
		public static void Remove(string fileOrHash)
		{
			if (shaders.ContainsKey (fileOrHash))
				Remove (shaders [fileOrHash]);
		}
	}
}

