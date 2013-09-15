using System;

namespace hgame1.Graphics.Particles
{
	public interface IParticleEmitter
	{
		Particle[] Emit (double time);
		float PPS { get; set; }
	}
}

