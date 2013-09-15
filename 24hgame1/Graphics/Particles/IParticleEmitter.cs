using System;

namespace hgame1.Graphics.Particles
{
	public interface IParticleEmitter
	{
		Particle[] Emit ();
		bool UpdateParticle (Particle particle, double time);
		float PPS { get; set; }
	}
}

