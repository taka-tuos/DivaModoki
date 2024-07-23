using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace DivaModoki
{
    internal static class Render
    {
        static Mutex bmutex;
        static int VertexBufferObject;
        static int VertexArrayObject;
        static int ElementBufferObject;
        static Shader? shader;

        static Vector2i wsize = new Vector2i(1280, 720);

        static Random random = new Random();

        struct RenderImage
        {
            public Texture tex;
            public Vector4i dst;
            public Vector4i src;
        }

        static RenderImage[] renders;
        static List<RenderImage> targets;

        static Render()
        {
            bmutex = new Mutex();
            renders = new RenderImage[0];
            targets = new List<RenderImage>();
        }

        public static void OnLoad()
        {
            shader = new Shader("shader.vert", "shader.frag");

            VertexBufferObject = GL.GenBuffer();
            VertexArrayObject = GL.GenVertexArray();
            ElementBufferObject = GL.GenBuffer();

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
        }

        public static void OnUnload()
        {
            shader?.Dispose();
        }

        static void DrawBuffer(Texture? tex, float[] vertices, uint[] indices)
        {
            if (tex != null && shader != null)
            {
                tex.Use();

                GL.BindVertexArray(VertexArrayObject);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
                GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StreamDraw);

                int vertexLocation = shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                int texCoordLocation = shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StreamDraw);

                shader.Use();

                GL.BindVertexArray(VertexArrayObject);
                GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }

        public static void OnRenderFrame()
        {
            RenderImage[] images;

            lock (bmutex)
            {
                images = renders.ToArray();
            }

            List<float> vertices = [];
            List<uint> indices = [];

            Texture? ntex = null;

            int idx = 0;

            foreach (RenderImage r in renders)
            {
                if (ntex == null || !r.tex.Equals(ntex))
                {
                    DrawBuffer(ntex, vertices.ToArray(), indices.ToArray());

                    ntex = r.tex;
                    vertices = [];
                    indices = [];
                    idx = 0;
                }

                if (ntex == null) break;

                int x = r.dst.X;
                int y = r.dst.Y;
                int w = r.dst.Z;
                int h = r.dst.W;

                float u1 = r.src.X / (float)ntex.Width;
                float u2 = u1 + r.src.Z / (float)ntex.Width;

                float v1 = r.src.Y / (float)ntex.Height;
                float v2 = v1 + r.src.W / (float)ntex.Height;

                float nx1 = x / (float)wsize.X * 2.0f - 1.0f;
                float nx2 = (x + w) / (float)wsize.X * 2.0f - 1.0f;

                // 逆にして普通の座標系と一致させる
                float ny1 = (wsize.Y - y) / (float)wsize.Y * 2.0f - 1.0f;
                float ny2 = (wsize.Y - (y + h)) / (float)wsize.Y * 2.0f - 1.0f;

                float[] pvertices = new float[20];
                uint[] pindices = new uint[6];

                pvertices[0] = nx1;
                pvertices[1] = ny1;
                pvertices[2] = 0.0f;
                pvertices[3] = u1;
                pvertices[4] = v1;

                pvertices[5] = nx1;
                pvertices[6] = ny2;
                pvertices[7] = 0.0f;
                pvertices[8] = u1;
                pvertices[9] = v2;

                pvertices[10] = nx2;
                pvertices[11] = ny2;
                pvertices[12] = 0.0f;
                pvertices[13] = u2;
                pvertices[14] = v2;

                pvertices[15] = nx2;
                pvertices[16] = ny1;
                pvertices[17] = 0.0f;
                pvertices[18] = u2;
                pvertices[19] = v1;

                vertices.AddRange(pvertices);

                pindices[0] = (uint)idx * 4 + 0;
                pindices[1] = (uint)idx * 4 + 1;
                pindices[2] = (uint)idx * 4 + 3;
                pindices[3] = (uint)idx * 4 + 1;
                pindices[4] = (uint)idx * 4 + 2;
                pindices[5] = (uint)idx * 4 + 3;

                indices.AddRange(pindices);

                idx++;
            }

            DrawBuffer(ntex, vertices.ToArray(), indices.ToArray());
        }

        public static void DrawImage(Texture tex, Vector4i dst, Vector4i src)
        {
            RenderImage r;
            r.dst = dst;
            r.src = src;
            r.tex = tex;

            targets.Add(r);
        }

        public static void DrawImage(Texture tex, Vector4i dst)
        {
            Vector4i src;
            src.X = 0;
            src.Y = 0;
            src.Z = tex.Width;
            src.W = tex.Height;

            DrawImage(tex, dst, src);
        }

        public static void RenderFlush()
        {
            lock(bmutex)
            {
                if (targets.Count > 0)
                {
                    renders = new RenderImage[targets.Count];
                    renders = targets.ToArray();
                    targets.Clear();
                }
            }
        }
    }
}
