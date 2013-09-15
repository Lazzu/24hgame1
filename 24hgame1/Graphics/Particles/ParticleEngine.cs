using System;
using System.Collections.Generic;
using OpenTK;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace hgame1.Graphics.Particles
{
	public static class ParticleEngine
	{

		public static List<Particle> Particles = new List<Particle>();
		public static List<IParticleEmitter> Emitters = new List<IParticleEmitter>();

		public static void Init(GameWindow gw)
		{
			gw.RenderFrame += HandleRenderFrame;
			gw.UpdateFrame += HandleUpdateFrame;
		}

		static void HandleRenderFrame (object sender, FrameEventArgs e)
		{
			foreach (var particle in Particles) {
				particle.Draw ();
			}
		}

		static void HandleUpdateFrame (object sender, FrameEventArgs e)
		{
			// Collect dead particles to this stack
			ConcurrentStack<Particle> dead = new ConcurrentStack<Particle>();

			// Update all particles
			Parallel.ForEach (Particles, (particle) => {
				if(!particle.Update (e.Time))
				{
					// If it returned false, it's dead
					dead.Push (particle);
				}
			});

			// Remove dead particles
			foreach (var particle in dead) {
				Particles.Remove (particle);
			}

			// Emit new particles
			foreach (var emitter in Emitters) {
				Particle[] p = emitter.Emit (e.Time);
				if(p != null)
					Particles.AddRange (p);
			}
		}


	}
}

