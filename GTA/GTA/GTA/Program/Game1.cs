using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            TargetElapsedTime = TimeSpan.FromSeconds(1/30.0);

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = false;
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

            else if (key.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F))
            {
                if (!KeyDown)
                {
                    KeyDown = true;
                    _graphics.ToggleFullScreen();
                }
            }

            if (key.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.F))
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

            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your update logic here

            _world.Update(gameTime.ElapsedGameTime);

            base.Update(gameTime);
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
