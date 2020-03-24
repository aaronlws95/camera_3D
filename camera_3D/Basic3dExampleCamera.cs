using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace camera_3D
{
    public class Basic3dExampleCamera
    {
        private GraphicsDevice graphicsDevice = null;
        private GameWindow gameWindow = null;

        private KeyboardState kbState = default(KeyboardState);

        public float MovementUnitsPerSecond { get; set; } = 10f;
        public float RotationRadiansPerSecond { get; set; } = 45f;

        public float fieldOfViewDegrees = 45;
        public float nearClipPlane = 0.1f;
        public float farClipPlane = 100f;

        private Vector3 startingPos;
        private Vector3 startingTargetPosToLookAt;

        public Basic3dExampleCamera(GraphicsDevice gfxDevice, GameWindow window, Vector3 pos, Vector3 lookAtTarget)
        {
            graphicsDevice = gfxDevice;
            gameWindow = window;
            startingPos = pos;
            Position = startingPos;
            startingTargetPosToLookAt = lookAtTarget;
            TargetPositionToLookAt = startingTargetPosToLookAt;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfViewDegrees), (float)gfxDevice.Viewport.Width / (float)gfxDevice.Viewport.Height, nearClipPlane, farClipPlane);
            ReCreateWorldAndView();
        }

        private Matrix cameraWorld = Matrix.Identity;
        private Matrix viewMatrix = Matrix.Identity;
        private Matrix projectionMatrix = Matrix.Identity;

        public Vector3 Position
        {
            set
            {
                cameraWorld.Translation = value;
                ReCreateWorldAndView();
            }
            get { return cameraWorld.Translation; }
        }

        public Vector3 LookAtDirection
        {
            set
            {
                cameraWorld = Matrix.CreateWorld(cameraWorld.Translation, value, cameraWorld.Up);
                ReCreateWorldAndView();
            }
            get { return cameraWorld.Forward; }
        }

        public Vector3 TargetPositionToLookAt
        {
            set
            {
                cameraWorld = Matrix.CreateWorld(cameraWorld.Translation, Vector3.Normalize(value - cameraWorld.Translation), cameraWorld.Up);
                ReCreateWorldAndView();
            }
        }

        public Matrix World
        {
            get
            {
                return cameraWorld;
            }
            set
            {
                cameraWorld = value;
                viewMatrix = Matrix.CreateLookAt(cameraWorld.Translation, cameraWorld.Forward + cameraWorld.Translation, cameraWorld.Up);
            }
        }

        public Matrix View
        {
            get
            {
                return viewMatrix;
            }
        }

        public Matrix Projection
        {
            get
            {
                return projectionMatrix;
            }
        }

        public void ResetCamera()
        {
            cameraWorld = Matrix.Identity;
            Position = startingPos;
            TargetPositionToLookAt = startingTargetPosToLookAt;
        }

        private void ReCreateWorldAndView()
        {
            cameraWorld = Matrix.CreateWorld(cameraWorld.Translation, cameraWorld.Forward, cameraWorld.Up);
            viewMatrix = Matrix.CreateLookAt(cameraWorld.Translation, cameraWorld.Forward + cameraWorld.Translation, cameraWorld.Up);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardControl(gameTime);

            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.R) == true)
            {
                ResetCamera();
            }

            kbState = kstate;
        }

        private void KeyboardControl(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.W))
            {
                RotateUp(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.S) == true)
            {
                RotateDown(gameTime);
            }

            if (kstate.IsKeyDown(Keys.A) == true)
            {
                RotateLeft(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.D) == true)
            {
                RotateRight(gameTime);
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                MoveForward(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.Down) == true)
            {
                MoveBackward(gameTime);
            }

            if (kstate.IsKeyDown(Keys.Left) == true)
            {
                MoveLeft(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.Right) == true)
            {
                MoveRight(gameTime);
            }

            if (kstate.IsKeyDown(Keys.Q) == true)
            {
                MoveUp(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.E) == true)
            {
                MoveDown(gameTime);
            }
            if (kstate.IsKeyDown(Keys.Z) == true)
            {
                RotateRollCounterClockwise(gameTime);
            }
            else if (kstate.IsKeyDown(Keys.C) == true)
            {
                RotateRollClockwise(gameTime);
            }

            kbState = kstate;
        }

        public void MoveForward(GameTime gameTime)
        {
            Position += (cameraWorld.Forward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveBackward(GameTime gameTime)
        {
            Position += (cameraWorld.Backward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveLeft(GameTime gameTime)
        {
            Position += (cameraWorld.Left * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveRight(GameTime gameTime)
        {
            Position += (cameraWorld.Right * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveUp(GameTime gameTime)
        {
            Position += (cameraWorld.Up * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void MoveDown(GameTime gameTime)
        {
            Position += (cameraWorld.Down * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void RotateUp(GameTime gameTime)
        {
            var radians = RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix matrix = Matrix.CreateFromAxisAngle(cameraWorld.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }

        public void RotateDown(GameTime gameTime)
        {
            var radians = -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix matrix = Matrix.CreateFromAxisAngle(cameraWorld.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }

        public void RotateLeft(GameTime gameTime)
        {
            var radians = RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix matrix = Matrix.CreateFromAxisAngle(cameraWorld.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }

        public void RotateRight(GameTime gameTime)
        {
            var radians = -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Matrix matrix = Matrix.CreateFromAxisAngle(cameraWorld.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }

        public void RotateRollClockwise(GameTime gameTime)
        {
            var radians = RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = cameraWorld.Translation;
            cameraWorld *= Matrix.CreateFromAxisAngle(cameraWorld.Forward, MathHelper.ToRadians(radians));
            cameraWorld.Translation = pos;
            ReCreateWorldAndView();
        }

        public void RotateRollCounterClockwise(GameTime gameTime)
        {
            var radians = -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = cameraWorld.Translation;
            cameraWorld *= Matrix.CreateFromAxisAngle(cameraWorld.Forward, MathHelper.ToRadians(radians));
            cameraWorld.Translation = pos;
            ReCreateWorldAndView();
        }
    }
}
