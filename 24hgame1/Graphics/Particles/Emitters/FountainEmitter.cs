using System;
using OpenTK;

namespace hgame1.Graphics.Particles.Emitters
{
	public class FountainEmitter : IParticleEmitter
	{
		public FountainEmitter ()
		{

		}

		double leftovers = 0;
				
		Particle CreateParticle(double time)
		{
			return new Particle {
				Position = new Vector2(0,0),
				Color = new Vector4(1,1,1,1),
				Velocity = 1,
				Direction = 0,
				Size = 10,
				Life = 10
			};
		}

		bool UpdateParticle(Particle particle, double time)
		{
			return false;
		}

		#region IParticleEmitter implementation

		public float PPS {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}

		public Particle[] Emit (double time)
		{
			double nParticles = (PPS / time) + leftovers;
			int n = (int)nParticles;
			leftovers = nParticles - n;

			Particle[] particles = new Particle[n];

			for(int i=0; i<n; i++)
			{
				particles [i] = CreateParticle (time);
				particles [i].Updaters += UpdateParticle;
			}

			return null;
		}



		#endregion
	}
}

