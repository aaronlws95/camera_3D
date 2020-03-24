﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace camera_3D
{
    public class Ship
    {
        public Vector3 Position;
        private Model model;
        private Matrix World;

        public Ship(Model loadModel, Vector3 pos)
        {
            model = loadModel;
            Position = pos;
            World = Matrix.CreateTranslation(Position);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = World;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }
    }
}
