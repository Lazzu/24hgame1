using System;
using System.Collections.Generic;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.Models;
using OpenTK;

namespace hgame1.Graphics.Rendering
{
	public static class ModelRenderer
	{
		static Dictionary<ShaderProgram,ShaderRenderGroup> shaderRenderGroups = new Dictionary<ShaderProgram, ShaderRenderGroup>();

		public static void AddToQueue(Model model, ref Matrix4 modelMatrix)
		{
			// Check if shader group exists
			if( ! shaderRenderGroups.ContainsKey(model.Shader))
			{
				shaderRenderGroups.Add (model.Shader, new ShaderRenderGroup ());
			}

			// Check if texture group exists
			if( ! shaderRenderGroups[model.Shader].TextureGroups.ContainsKey(model.Textures[0]) )
			{
				shaderRenderGroups [model.Shader].TextureGroups.Add (model.Textures [0], new List<ModelRenderProperties>());
			}

			// Add model in a texture group in a shader group
			shaderRenderGroups [model.Shader].TextureGroups [model.Textures [0]].Add (new ModelRenderProperties(model, ref modelMatrix));
		}

		public static void Render()
		{
			foreach (var shaderGroup in shaderRenderGroups) {

				ShaderProgram shader = shaderGroup.Key;

				shader.Enable ();

				shader.SendUniform ("mP", ref Camera.ProjectionMatrix);
				shader.SendUniform ("mV", ref Camera.ViewMatrix);

				foreach (var textureGroup in shaderGroup.Value.TextureGroups) {

					textureGroup.Key.Bind ();

					foreach (var modelProperties in textureGroup.Value) {

						ModelRenderProperties properties = modelProperties;

						Camera.UseModelMatrix (ref properties.ModelMatrix);

						shader.SendUniform ("mM", ref properties.ModelMatrix);
						shader.SendUniform ("mN", ref Camera.NormalMatrix);

						properties.Model.RawRender ();

					}
				}
			}

			// Clear the queue
			shaderRenderGroups.Clear ();
		}
	}
}

