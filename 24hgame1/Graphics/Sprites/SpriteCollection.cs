using System;
using System.Collections.Generic;

namespace hgame1.Graphics.Sprites
{
	public class SpriteCollection : IDictionary<string, Sprite>
	{
		public Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

		public SpriteCollection ()
		{
		}

		public Sprite this[string name]
		{
			get {
				return Sprites [name];
			}
            set
            {
                throw new NotImplementedException();
            }
		}


		#region IDictionary implementation
		public void Add (string key, Sprite value)
		{
			Sprites.Add (key, value);
		}
		public bool ContainsKey (string key)
		{
			return Sprites.ContainsKey (key);
		}
		public bool Remove (string key)
		{
			return Sprites.Remove (key);
		}
		public bool TryGetValue (string key, out Sprite value)
		{
			return Sprites.TryGetValue (key, out value);
		}
		public ICollection<string> Keys {
			get {
				return Sprites.Keys;
			}
		}
		public ICollection<Sprite> Values {
			get {
				return Sprites.Values;
			}
		}
		#endregion
		#region ICollection implementation
		public void Add (KeyValuePair<string, Sprite> item)
		{
			Sprites.Add (item.Key, item.Value);
		}
		public void Clear ()
		{
			Sprites.Clear ();
		}
		public bool Contains (KeyValuePair<string, Sprite> item)
		{
			return Sprites.ContainsKey (item.Key) && Sprites.ContainsValue (item.Value);
		}
		public void CopyTo (KeyValuePair<string, Sprite>[] array, int arrayIndex)
		{
			throw new NotImplementedException ();
		}
		public bool Remove (KeyValuePair<string, Sprite> item)
		{
			throw new NotImplementedException ();
		}
		public int Count {
			get {
				return Sprites.Count;
			}
		}
		public bool IsReadOnly {
			get {
				throw new NotImplementedException ();
			}
		}
		#endregion
		#region IEnumerable implementation
		public IEnumerator<KeyValuePair<string, Sprite>> GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
		#endregion
		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

