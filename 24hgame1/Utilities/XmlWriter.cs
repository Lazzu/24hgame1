using System;
using System.IO;
using System.Xml.Serialization;

namespace hgame1.Utilities
{
	public class XmlWriter
	{
		/// <summary>
		/// Write object to xml string
		/// </summary>
		/// <param name="obj">Object.</param>
		public string String(object obj)
		{
			using (Stream s = Stream (obj))
			{
				using (StreamReader r = new StreamReader(s))
				{
					return r.ReadToEnd ();
				}
			}
		}

		/// <summary>
		/// Write object to xml file
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="file">File.</param>
		public void File(object obj, string file)
		{
			using(FileStream fs = new FileStream(file, FileMode.Create))
			{
				Stream (obj, fs);
			}
		}

		/// <summary>
		/// Write object to xml stream
		/// </summary>
		/// <param name="obj">Object.</param>
		public Stream Stream(object obj)
		{
			MemoryStream stream = new MemoryStream ();

			Stream (obj, stream);

			return stream;
		}

		/// <summary>
		/// Write object to xml stream
		/// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="stream">Stream.</param>
		public void Stream(object obj, Stream stream)
		{
			XmlSerializer s = new XmlSerializer(obj.GetType());
			using(StreamWriter w = new StreamWriter(stream))
			{
				s.Serialize (w, obj);
			}
		}
	}
}

