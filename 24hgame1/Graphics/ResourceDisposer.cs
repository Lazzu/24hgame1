using System;
using System.Collections.Concurrent;

namespace hgame1.Graphics
{
	public static class ResourceDisposer
	{
		public static ConcurrentQueue<IDisposable> DisposingQueue = new ConcurrentQueue<IDisposable>();

		public static void DisposeQueue()
		{
			if(DisposingQueue.Count > 0)
			{
				Console.WriteLine ("GLDisposer Disposing resources.");
				while(DisposingQueue.Count > 0)
				{
					IDisposable obj;
					DisposingQueue.TryDequeue (out obj);
					if(obj != null)
					{
						obj.Dispose ();
					}
				}
			}


		}
	}
}

