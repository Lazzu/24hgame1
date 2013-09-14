using System;
using System.IO;
using System.Xml.Serialization;

namespace hgame1.Utilities
{
	/// <summary>
	/// Quick xml reader class.
	/// </summary>
	public class XmlReader
	{
		/// <summary>
		/// Reads a file to an object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="file">File.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T ReadFile<T>(string file)
		{
			using(FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				return ReadStream<T> (stream);
			}
		}

		/// <summary>
		/// Reads a XML byte array to object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="xml">Xml.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T ReadXML<T>(byte[] xml)
		{
			using(MemoryStream stream = new MemoryStream(xml))
			{
				return ReadStream<T> (stream);
			}
		}

		/// <summary>
		/// Reads a XML string to object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="xml">Xml.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T ReadXML<T>(string xml, System.Text.Encoding encoder)
		{
			using(MemoryStream stream = new MemoryStream(encoder.GetBytes(xml)))
			{
				return ReadStream<T> (stream);
			}
		}

		/// <summary>
		/// Reads the stream containing xml to object.
		/// </summary>
		/// <returns>The object.</returns>
		/// <param name="stream">Stream.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T ReadStream<T>(Stream stream)
		{
			XmlSerializer r = new XmlSerializer (typeof(T));
			return (T)r.Deserialize (stream);
		}
	}
}

