using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace camera_3D
{
    class MyShape
    {
        private BasicEffect effect;
        private Color color;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
        public MyShape(GraphicsDevice gfxDevice, Vector3 pos, Color? setColor=null)
        {          
            color = setColor ?? Color.Green;
            effect = new BasicEffect(gfxDevice);
            MakeCube(gfxDevice, pos);
        }

        public void MakeCube(GraphicsDevice gfxDevice, Vector3 pos)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[8];
            short[] indices = new short[36];
            vertexBuffer = new VertexBuffer(gfxDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(gfxDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
 
            float width = 2f;
            float height = 2f;
            float depth = 2f;

            pos = new Vector3(pos.X - width / 2, pos.Y + height / 2, 0);

            vertices[0] = new VertexPositionColor(pos, color);
            vertices[1] = new VertexPositionColor(pos + new Vector3(width, 0, 0), color);
            vertices[2] = new VertexPositionColor(pos + new Vector3(width, -height, 0), color);
            vertices[3] = new VertexPositionColor(pos + new Vector3(0, -height, 0), color);
            vertices[4] = new VertexPositionColor(pos + new Vector3(0, 0, depth), color);
            vertices[5] = new VertexPositionColor(pos + new Vector3(width, 0, depth), color);
            vertices[6] = new VertexPositionColor(pos + new Vector3(width, -height, depth), color);
            vertices[7] = new VertexPositionColor(pos + new Vector3(0, -height, depth), color);

            indices[0] = 0; indices[1] = 1; indices[2] = 2;
            indices[3] = 0; indices[4] = 3; indices[5] = 2;
            indices[6] = 4; indices[7] = 0; indices[8] = 3;
            indices[9] = 4; indices[10] = 7; indices[11] = 3;
            indices[12] = 3; indices[13] = 7; indices[14] = 6;
            indices[15] = 3; indices[16] = 6; indices[17] = 2;
            indices[18] = 1; indices[19] = 5; indices[20] = 6;
            indices[21] = 1; indices[22] = 5; indices[23] = 2;
            indices[24] = 4; indices[25] = 5; indices[26] = 6;
            indices[27] = 4; indices[28] = 7; indices[29] = 6;
            indices[30] = 0; indices[31] = 1; indices[32] = 5;
            indices[33] = 0; indices[34] = 4; indices[35] = 5;

            vertexBuffer.SetData<VertexPositionColor>(vertices);
            indexBuffer.SetData(indices);
        }

        public void Draw(GraphicsDevice gfxDevice, Matrix view, Matrix projection)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;

            gfxDevice.SetVertexBuffer(vertexBuffer);
            gfxDevice.Indices = indexBuffer;

            RasterizerState rasterizerState = new RasterizerState
            {
                CullMode = CullMode.None
            };
            gfxDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gfxDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 20);
            }
        }
    }
}
