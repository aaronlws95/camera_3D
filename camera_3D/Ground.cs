using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace camera_3D
{
    class Ground
    {
        private VertexPositionTexture[] floorVerts;
        private BasicEffect effect;
        private Texture2D texture;

        public Ground(GraphicsDevice gfxDevice, Texture2D loadTexture, int repetitions=20)
        {
            floorVerts = new VertexPositionTexture[6];

            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            effect = new BasicEffect(gfxDevice);
            texture = loadTexture;
        }

        public void Draw(GraphicsDevice gfxDevice, Matrix view, Matrix projection)
        {
            effect.View = view;
            effect.Projection = projection;
            effect.TextureEnabled = true;
            effect.Texture = texture;
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                gfxDevice.DrawUserPrimitives(PrimitiveType.TriangleList, floorVerts, 0, 2);
            }
        }
    }
}
