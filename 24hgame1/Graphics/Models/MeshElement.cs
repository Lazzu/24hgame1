using System;
using OpenTK;
using System.Runtime.InteropServices;

namespace hgame1.Graphics.Models
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MeshElement
	{
		public Vector3 Vertex;
		public Vector3 Normal;
		public Vector2 TexCoord;

		public override bool Equals (object obj)
		{
			return Equals((MeshElement)obj);
		}

		public bool Equals (MeshElement obj)
		{
			return 
				Vertex.Equals (obj.Vertex) &&
				Normal.Equals (obj.Normal) &&
				TexCoord.Equals (obj.TexCoord);
		}

		public static int SizeInBytes {
			get {
				return (Vector3.SizeInBytes * 2) + Vector2.SizeInBytes;
			}
		}
	}
}

