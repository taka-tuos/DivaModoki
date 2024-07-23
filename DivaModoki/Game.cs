using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Keys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

namespace DivaModoki
{
    internal class Game : GameWindow
    {
        public Game(int width, int height, string title) :
            base(GameWindowSettings.Default,
                new NativeWindowSettings()
                {
                    ClientSize = (width, height),
                    Title = title
                })
        {
            // nothing to do
        }

        Texture texture;
        Texture number;

        Random random = new Random(3872);

        Stopwatch sw_1sec;
        Stopwatch sw_chart;

        int fpscounter = 0;
        int fpsvalue = 0;

        Chart chart;

        protected override void OnLoad()
        {
            base.OnLoad();

            ChartLoader loader = new ChartLoader("chart.txt");
            loader.Load(out chart);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            //Code goes here
            texture = new Texture("gt8x.png");
            number = new Texture("number.png");

            Render.OnLoad();

            //Matrix4 view = Matrix4.CreateOrthographic(2.0f, 2.0f, 0.0f, 4096.0f);

            sw_1sec = Stopwatch.StartNew();
            sw_chart = Stopwatch.StartNew();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            Render.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            ParticleManager.ParticlesFlush();
            Render.OnRenderFrame();

            SwapBuffers();
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            chart.PeekNotes(sw_chart.ElapsedMilliseconds, 1000, out SortedDictionary<int, Note> notes);

            foreach (KeyValuePair<int, Note> kvp in notes)
            {
                Vector4i v;
                v.X = (int)(kvp.Key / 1000.0 * ClientSize.X);
                v.Y = 64;
                v.Z = 32;
                v.W = 32;

                Render.DrawImage(texture, v);

                if(kvp.Key >= 0 && kvp.Key < 5)
                {
                    Particle p = new Particle(texture, (p, t, o) =>
                    {
                        if (p.Watch.ElapsedMilliseconds > 300) return false;

                        Vector4i v1;

                        int siz = 64 - (int)(p.Watch.ElapsedMilliseconds / 300.0 * 64);

                        v1.X = - siz / 2;
                        v1.Y = 64 + 16 - (siz / 2);
                        v1.Z = siz;
                        v1.W = siz;

                        Render.DrawImage(texture, v1);

                        return true;
                    }, null);

                    ParticleManager.AddParticle(p);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                int fpsdigit = (fpsvalue / (int)Math.Pow(10, i)) % 10;

                Vector4i dst, src;

                dst.X = 7 * (3 - i);
                dst.Y = 0;
                dst.Z = 7;
                dst.W = 9;

                src.X = fpsdigit * 7;
                src.Y = 0;
                src.Z = 7;
                src.W = 9;

                Render.DrawImage(number, dst, src);
            }

            Render.RenderFlush();

            fpscounter++;

            if (sw_1sec.ElapsedMilliseconds > 1000)
            {
                sw_1sec.Restart();
                fpsvalue = fpscounter;
                fpscounter = 0;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }
    }
}
