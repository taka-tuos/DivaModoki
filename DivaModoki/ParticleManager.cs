using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivaModoki
{
    internal static class ParticleManager
    {
        static List<Particle> particles;

        static ParticleManager()
        {
            particles = new List<Particle>();
        }

        public static void ParticlesFlush()
        {
            lock (particles)
            {
                for (int i = particles.Count - 1; i >= 0; i--)
                {
                    bool res = particles[i].Draw();

                    if (!res) particles.RemoveAt(i);
                }
            }
        }

        public static void ParticlesClear()
        {
            lock (particles)
            {
                particles.Clear();
            }
        }

        public static void AddParticle(Particle particle)
        {
            lock (particles)
            {
                particles.Add(particle);
            }
        }
    }
}
