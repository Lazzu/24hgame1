using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace hgame1.Graphics.Shaders
{
	public class ShaderPreprocessor
	{
		public static Regex IncludeRegex = new Regex("^#include (.*);$", 
		                                             RegexOptions.Compiled | 
		                                             RegexOptions.Multiline);

		Dictionary<string, string> keyValues = new Dictionary<string, string>();

		public void Add(string key, string value)
		{
			if (keyValues.ContainsKey (key))
				return;

			keyValues.Add (key, value);
		}

		public string Process(string shaderSource)
		{
			// Remove any \r for it to work on many platforms
			shaderSource = shaderSource.Replace ("\r\n", "\n");

			// Get matches 
			MatchCollection matches = IncludeRegex.Matches (shaderSource);

			// Go through all matches
			foreach(Match match in matches)
			{
				// Check if the match was a success
				if(match.Success)
				{
					// Replace the #include for the shader source to pass the GL driver precompiler
					shaderSource = IncludeRegex.Replace (shaderSource, "// Found included " + match.Groups [1].Value + ";");

					// Include the included source in the end of the source code
					shaderSource += "\n" + Process(ReadFile (match.Groups [1].Value));
				}
			}

			// Then replace all keys with values in the source
			foreach(var kvp in keyValues)
			{
				shaderSource = shaderSource.Replace (kvp.Key, kvp.Value);
			}

			// Return the preprocessed shader source
			return shaderSource;
		}

		string ReadFile(string file)
		{
			using(FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using(StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
				{
					return reader.ReadToEnd ();
				}
			}
		}
	}
}

