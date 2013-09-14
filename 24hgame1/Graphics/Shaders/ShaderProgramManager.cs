using System;
using System.Collections.Generic;
using hgame1.Utilities;

namespace hgame1.Graphics.Shaders
{
	public static class ShaderProgramManager
	{
		static Dictionary<string, ShaderProgram> programs = new Dictionary<string, ShaderProgram>();

		public static void Add(string name, ShaderProgram program)
		{
			if (ProgramExists (name))
				throw new Exception ("Can not add shader with a same name");

			programs.Add (name, program);
		}

		public static ShaderProgram LoadXml(string name, string file)
		{
			// Check if name already exists
			if (ProgramExists (name))
				throw new ArgumentException ("A shader program named " + name + " already exists. Only unique names allowed!");

			// Load the shader from the file
			ShaderProgram p = LoadXml (file);

			// Add the program to the dictionary using the name as key
			programs.Add (name, p);

			return p;
		}

		public static ShaderProgram LoadXml(string file)
		{
			file = Settings.CurrentSettings.GetShaderPath (file);

			if(ProgramExists(file))
			{
				return programs [file];
			}

			// Get the shader settings object
			ShaderProgramXml shaderSettings = Xml.Read.ReadFile<ShaderProgramXml> (file);

			// Load using settings
			ShaderProgram program = LoadXml (shaderSettings);

			// Add the program in the dictionary using the file name as key
			programs.Add (file, program);

			return program;
		}

		public static ShaderProgram LoadXml(ShaderProgramXml shaderSettings)
		{
			// Create the shader program
			ShaderProgram program = new ShaderProgram ();

			// Attach shaders to program
			foreach (var shader in shaderSettings.Shaders) {
				Shader s;

				if (shader.Inline)
					s = ShaderManager.FromSource (shader.Data, shader.Type);
				else
					s = ShaderManager.FromFile (shader.Data, shader.Type);

				program.Attach (s);
			}

			// Link the program
			program.Link ();

			// Find the uniforms
			program.FindUniforms (shaderSettings.Uniforms.ToArray ());

			// Add the shader program to the dictionary using name from settings
			programs.Add (shaderSettings.Name, program);

			return program;
		}

		public static ShaderProgram Get(string name)
		{
			if (ProgramExists (name))
				return programs [name];

			return null;
		}

		public static bool ProgramExists(string name)
		{
			return programs.ContainsKey (name);
		}

		public static void Remove(string name)
		{
			programs.Remove (name);
		}

		public static void Remove(ShaderProgramXml settings)
		{
			programs.Remove (settings.Name);
		}
	}
}

