using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivaModoki
{
    internal class Particle(Texture tex, Particle.OnDrawCallback onDraw, object? payload)
    {
        public delegate bool OnDrawCallback(Particle sender, Texture tex, object? payload);

        public OnDrawCallback OnDraw = onDraw;

        object? payload = payload;
        Texture tex = tex;

        public Stopwatch Watch = Stopwatch.StartNew();

        public bool Draw()
        {
            return OnDraw(this, tex, payload);
        }
    }
}
