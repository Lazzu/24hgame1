using System;
using OpenTK;

namespace hgame1.Graphics
{
	public static class Camera
	{
		public static Matrix4 ProjectionMatrix;
		public static Matrix4 ViewMatrix;
		public static Matrix4 MVPMatrix;

		static GameWindow gameW;

		static Vector3 positionTarget;
		static Vector3 position;
		public static Vector2 Position {
			get { return position.Xy; }
			private set {
				position = new Vector3(value);
			}
		}

		public static void MoveAt(Vector2 pos)
		{
			positionTarget = new Vector3(pos);
		}

		public static void MoveAtInstantly(Vector2 pos)
		{
			positionTarget = new Vector3(pos);
			position = new Vector3(pos);
		}

		public static void Init(GameWindow gw)
		{
			gameW = gw;
			gw.Resize += HandleResize;
			gw.UpdateFrame += HandleUpdateFrame;
		}

		static void HandleResize (object sender, EventArgs e)
		{
			ProjectionMatrix = Matrix4.CreateOrthographicOffCenter (0, gameW.Width, gameW.Height, 0, 0, 1);
		}

		static void HandleUpdateFrame (object sender, FrameEventArgs e)
		{
			position += (positionTarget - position) / 8;

			Matrix4.CreateTranslation (ref position, out ViewMatrix);
		}
	}
}

