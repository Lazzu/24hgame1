using System;
using OpenTK;

namespace hgame1.Graphics
{
	public static class Camera
	{
		public static Matrix4 ProjectionMatrix = Matrix4.Identity;
		public static Matrix4 ViewMatrix = Matrix4.Identity;
		public static Matrix4 MVPMatrix = Matrix4.Identity;
		public static Matrix4 NormalMatrix = Matrix4.Identity;

		static GameWindow gameW;

		static Vector3 positionTarget;
		static Vector3 position;
		public static Vector2 Position {
			get { return position.Xy; }
			private set {
				position = new Vector3(value);
			}
		}

		public static Box2 ScreenBounds {
			get {
				Vector2 size = new Vector2 (gameW.Width, gameW.Height);

				Box2 bounds = new Box2 (position.Xy, position.Xy + size);

				return bounds;
			}
		}

		public static void Move(Vector2 pos)
		{
			positionTarget -= new Vector3(pos);
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

		public static void UseModelMatrix(ref Matrix4 modelMatrix)
		{
			MVPMatrix = ProjectionMatrix * ViewMatrix * modelMatrix;
			NormalMatrix = ViewMatrix * modelMatrix;
			NormalMatrix.Invert ();
		}

		public static void Init(GameWindow gw)
		{
			gameW = gw;
			gw.Resize += HandleResize;
			gw.UpdateFrame += HandleUpdateFrame;
		}

		static void HandleResize (object sender, EventArgs e)
		{
			ProjectionMatrix = Matrix4.CreateOrthographicOffCenter (0, gameW.Width, gameW.Height, 0, 0, -1);
		}

		static void HandleUpdateFrame (object sender, FrameEventArgs e)
		{
			position += (positionTarget - position) / 4; // Calculate camera position easing

			position = -position;

			Matrix4.CreateTranslation (ref position, out ViewMatrix);

			position = -position;
		}
	}
}

