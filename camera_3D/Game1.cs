using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace camera_3D
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MyCamera cam;
        private MyModel ship;
        private MyShape cube;
        private Ground ground;
        private SpriteFont camSpriteFont;
        private string camTextPosition = "Camera Position: ({0}, {1}, {2})";
        private string camTextRotation = "Camera Rotation: ({0}, {1}, {2})";
        private float roll;
        private float yaw;
        private float pitch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ship = new MyModel(Content.Load<Model>("Ship"), new Vector3(0, 10, 5));
            ground = new Ground(GraphicsDevice, Content.Load<Texture2D>("checkerboard"), Vector3.Zero);
            cam = new MyCamera(GraphicsDevice, this.Window, new Vector3(0, 0, 10), Vector3.Zero);
            camSpriteFont = Content.Load<SpriteFont>("CamSpriteFont");
            cube = new MyShape(GraphicsDevice, Vector3.Zero, Color.Green);
        }

        protected override void UnloadContent()
        {
 
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Quaternion rotQ;
            cam.World.Decompose(out _, out rotQ, out _);
            MyMath.QuaternionToEuler(rotQ, out roll, out pitch, out yaw);
            cam.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            /* https://gamedev.stackexchange.com/questions/31616/spritebatch-begin-making-my-model-not-render-correctly */
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            ship.Draw(cam.View, cam.Projection);
            ground.Draw(GraphicsDevice, cam.View, cam.Projection);
            cube.Draw(GraphicsDevice, cam.View, cam.Projection);
            DrawText();
            base.Draw(gameTime);
        }

        private void DrawText()
        {
            spriteBatch.Begin();
            string camText = string.Format(camTextPosition, cam.Position.X, cam.Position.Y, cam.Position.Z);
            spriteBatch.DrawString(camSpriteFont, camText, new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(camSpriteFont, string.Format(camTextRotation, roll, pitch, yaw),
                                    new Vector2(0, camSpriteFont.MeasureString(camText).Y), Color.Black);
            spriteBatch.End();
        }
    }
}
