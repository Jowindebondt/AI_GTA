using System;
using System.Linq;
using System.Windows.Forms;
using GTA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace GTA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        readonly GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        private World _world;

        const int Width = 1600;
        const int Height = 900;

        public bool KeyDown { get; set; }

        private Viewport viewport;

        public Game1()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1/15.0);

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            _graphics.PreparingDeviceSettings += PreparingDeviceSettings;

            _world = World.GetInstance();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            var resOk = false;

            foreach (var dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Where(dm => dm.Width == Width && dm.Height == Height))
                resOk = true;

            if (resOk)
            {
                e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = false;
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = SurfaceFormat.Color;
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = Width;
                e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = Height;
            }
            else
            {
                MessageBox.Show("Cannot find requested resolution", "Severe Error");
                Exit();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            viewport = GraphicsDevice.Viewport;
            _world.Load(GraphicsDevice, Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void KeyPresses(KeyboardState key)
        {
            if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F))
            {
                if (!KeyDown)
                {
                    KeyDown = true;
                    _graphics.ToggleFullScreen();
                }
            }

            if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
            {
                if (!KeyDown)
                {
                    KeyDown = true;
                    World.GetInstance()._drawGraph = !World.GetInstance()._drawGraph;
                }
            }

            if (key.IsKeyDown(Keys.Up))
            {
                _world.UpdateThug(Keys.Up);
            }

            if (key.IsKeyDown(Keys.Left))
            {
                _world.UpdateThug(Keys.Left);
            }

            if (key.IsKeyDown(Keys.Right))
            {
                _world.UpdateThug(Keys.Right);
            }

            if (key.IsKeyDown(Keys.Down))
            {
                _world.UpdateThug(Keys.Down);
            }

            if (key.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F) || key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F1))
                KeyDown = false;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            var key = Keyboard.GetState();
            KeyPresses(key);

            var mouse = Mouse.GetState();
            MousePresses(mouse);

            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here
            _world.Update(elapsed);

            base.Update(gameTime);
        }

        private void MousePresses(MouseState mouse)
        {
            Random rand = new Random();
            if (mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                ClearPreviousNode();

                Node startnode = _world._graph.GetNodeFromPoint(((int)(_world.MovingEntities[1].Pos.X/32)* 32),
                                                                (int)(_world.MovingEntities[1].Pos.Y/32)* 32);

                Node endnode = _world._graph.GetNodeFromPoint((int)(mouse.X / 32)*32,
                                                                (int)(mouse.Y / 32)*32); // / 32 * 32!!!
                _world.MovingEntities[1].SteeringBehaviors.CreateListAStar(startnode, endnode);
                _world.endNode = endnode;
            }
        }

        private void ClearPreviousNode()
        {
            foreach (var node in _world._graph.getNodes())
                node.Previous = null;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(); 
            _world.Render(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
