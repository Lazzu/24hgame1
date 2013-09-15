using System;
using System.Collections.Generic;
using hgame1.Graphics.Textures;
using OpenTK;
using hgame1.Graphics.Shaders;
using hgame1.Graphics.Rendering;

namespace hgame1.Graphics.Models
{
	public class Model : IRenderable
	{
		public List<Mesh> Meshes = new List<Mesh>();
		public List<Texture> Textures = new List<Texture>();

		public ShaderProgram Shader;

		public Model ()
		{

		}

		public void Initialize()
		{
			if(Shader != null)
			{
				// Find shader uniforms
				Shader.FindUniforms (new string[]{
					"mP", "mV", "mM",// "mN" // Matrices
				});
			}

			foreach (var mesh in Meshes) {
				mesh.Upload ();
			}
		}

		#region IRenderable implementation
		public void QueueRender (ref Matrix4 modelMatrix)
		{
			ModelRenderer.AddToQueue (this, ref modelMatrix);
		}
		public void Render (ref Matrix4 modelMatrix)
		{
			Shader.Enable ();

			//Camera.UseModelMatrix (ref modelMatrix);

			Shader.SendUniform ("mP", ref Camera.ProjectionMatrix);
			Shader.SendUniform ("mV", ref Camera.ViewMatrix);
			Shader.SendUniform ("mM", ref modelMatrix);
			//Shader.SendUniform ("mN", ref Camera.NormalMatrix);

			//Textures [0].Bind ();

			for (int i = 0; i < Meshes.Count; i++) {
				Meshes [i].Render ();
			}

			//Textures [0].UnBind ();

			Shader.Disable ();
		}
		public void RawRender ()
		{
			for (int i = 0; i < Meshes.Count; i++) {
				Meshes [i].Render ();
			}
		}
		public void Update (double time)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

