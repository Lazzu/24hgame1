using System;

namespace hgame1.Utilities
{
	/// <summary>
	/// Our own little random number generator for generating consistent 
	/// random numbers between .net and mono.
	/// </summary>
	public class Random
	{
		// Uint seeds which will change every time a number get generated
		uint m_z, m_w;

		/// <summary>
		/// Gets the original seed used to seed this random number generator.
		/// </summary>
		/// <value>The original seed.</value>
		public int OriginalSeed {
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MGLALLib.Utilities.Random"/> class.
		/// </summary>
		public Random ()
		{
			// Prime numbers just for the kicks
			SetSeed ( DateTime.Today.Year * 3 +
			         DateTime.Today.Month * 5 +
			         DateTime.Today.Day * 7 +
			         DateTime.Today.Hour * 11 +
			         DateTime.Today.Minute * 13 +
			         DateTime.Today.Second * 17 +
			         DateTime.Today.Millisecond +
			         Environment.TickCount );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MGLALLib.Utilities.Random"/> class.
		/// </summary>
		/// <param name="seed">Predefined seed.</param>
		public Random (int seed)
		{
			// Use predefined seed
			SetSeed (seed);
		}

		/// <summary>
		/// Get the next random int
		/// </summary>
		public int Next()
		{
			return (int)(GetUint () / 2);
		}

		/// <summary>
		/// Get the next random int in between specified minimum and maximum.
		/// </summary>
		/// <param name="minimum">Minimum.</param>
		/// <param name="maximum">Maximum.</param>
		public int Next(int minimum, int maximum)
		{
			return (int)(NextDouble () * Math.Abs(maximum - minimum) + minimum);
		}

		/// <summary>
		/// Get the next random double in between specified minimum and maximum.
		/// </summary>
		/// <param name="minimum">double Minimum.</param>
		/// <param name="maximum">double Maximum.</param>
		public double NextDouble(double minimum, double maximum)
		{
			return NextDouble () * Math.Abs(maximum - minimum) + minimum;
		}

        /// <summary>
        /// Get the next random double in between specified minimum and maximum.
        /// </summary>
        /// <param name="minimum">int Minimum.</param>
        /// <param name="maximum">int Maximum.</param>
        public double NextDouble(int minimum, int maximum)
        {
            return NextDouble() * Math.Abs(maximum - minimum) + minimum;
        }

		/// <summary>
		/// Get the next random double
		/// </summary>
		public double NextDouble()
		{
			// Copy and paste from http://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation
			return (GetUint () + 1.0) * 2.328306435454494e-10;
		}

		/// <summary>
		/// Get the next random uint as base for all other values. 
		/// </summary>
		/// <returns>The uint.</returns>
		uint GetUint()
		{
			// Copy and paste from http://www.codeproject.com/Articles/25172/Simple-Random-Number-Generation
			m_z = 36969 * (m_z & 65535) + (m_z >> 16);
			m_w = 18000 * (m_w & 65535) + (m_w >> 16);
			return (m_z << 16) + m_w;
		}

		/// <summary>
		/// Set the seeds and warm up
		/// </summary>
		/// <param name="randomSeed">Random seed.</param>
		void SetSeed(int randomSeed)
		{
			OriginalSeed = randomSeed;

			// Set the seeds
			m_z = (uint)(randomSeed + int.MaxValue + 1); // If the given seed is negative, make sure it is 0 or above
			m_w = (uint)Math.Sqrt(randomSeed); // I just thought this might give some randomness

			// Warm up the seeds
			for(int i=0; i<5; i++)
			{
				// No real reason for assigning the m_z here. I felt like to do it like this.
				m_z = GetUint ();
			}
		}
	}
}

