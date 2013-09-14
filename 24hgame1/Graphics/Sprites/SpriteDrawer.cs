using System;
using OpenTK;
using System.Collections.Generic;
using hgame1.Graphics.Textures;
using hgame1.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;

namespace hgame1.Graphics.Sprites
{
	public static class SpriteDrawer
	{
		static Dictionary<ShaderProgram, Dictionary<Texture, List<SpriteDrawData>>> drawQueue = new Dictionary<ShaderProgram, Dictionary<Texture, List<SpriteDrawData>>>();
		static Dictionary<ShaderProgram, Dictionary<Texture, SpriteDrawParameters>> drawList = new Dictionary<ShaderProgram, Dictionary<Texture, SpriteDrawParameters>>();

		static uint vbo;

		static List<SpriteDrawData> buffer = new List<SpriteDrawData> ();
		static SpriteDrawData[] rawBuffer = new SpriteDrawData[1];

		public static void Initialize(GameWindow gw)
		{

		}

		public static void AddSprite(Sprite sprite, SpriteDrawData drawdata)
		{
			// If texture dictionary does not yet exist
			if (!drawQueue.ContainsKey (sprite.Shader))
				drawQueue.Add (sprite.Shader, new Dictionary<Texture, List<SpriteDrawData>> ());

			// If sprite list dictionary does not yet exist
			if (!drawQueue [sprite.Shader].ContainsKey (sprite.Texture))
				drawQueue [sprite.Shader].Add (sprite.Texture, new List<SpriteDrawData> ());

			// Add sprite to the sprite drawing queue
			drawQueue [sprite.Shader] [sprite.Texture].Add (drawdata);
		}

		static void Upload()
		{
			int offset = 0;

			// Generate drawing parameters
			foreach (var drawGroup in drawQueue) 
			{

				drawList.Add (drawGroup.Key, new Dictionary<Texture, SpriteDrawParameters>());

				foreach (var drawGroup2 in drawGroup.Value) 
				{
					int count = drawGroup.Value.Count;

					drawList[drawGroup.Key].Add (drawGroup2.Key, new SpriteDrawParameters (offset, count));

					buffer.AddRange (drawGroup2.Value);

					offset += count;
				}
			}

			rawBuffer = buffer.ToArray ();

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData (BufferTarget.ArrayBuffer, (IntPtr)(buffer.Count * SpriteDrawData.SizeInBytes), rawBuffer, BufferUsageHint.StreamDraw);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			// Clear the buffer list
			buffer.Clear ();

			// Clear the queue
			drawQueue.Clear ();
		}

		static void Render()
		{
			// Bind the vbo
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

			// Loop through all shaders
			foreach (var shader in drawList) 
			{
				// Enable the shader
				shader.Key.Enable ();

				// Loop through all textures
				foreach (var texture in shader.Value) 
				{
					// Get the params
					SpriteDrawParameters param = texture.Value;

					// Bind the texture
					texture.Key.Bind();

					// Draw the points using offsets in SpriteDrawParameters
					GL.DrawArrays (BeginMode.Points, param.Offset, param.Count);	

					// Bind the texture
					texture.Key.UnBind();
				}

				// Enable the shader
				shader.Key.Disable ();
			}

			// Unbind the texture to prevent graphical glitches


			// Disable the shader to prevent graphical glitches


			// Unbind the vbo
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			// Clear the draw list
			drawList.Clear ();
		}
	}
}

