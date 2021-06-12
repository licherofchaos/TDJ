using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Genbox.VelcroPhysics.Dynamics;
using IPCA.MonoGame;

namespace TDJ
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Scene _scene;
        private Player _player;
        private NPC _npc;
        private World _world;
        private Coin _coin;
        private Texture2D _background;
        public Player Player => _player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _world = new World(new Vector2(0, -9.82f));
            Services.AddService(_world);
            
            new KeyboardManager(this);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();

            Debug.SetGraphicsDevice(GraphicsDevice);

            new Camera(GraphicsDevice, height: 5f);
            Camera.LookAt(Camera.WorldSize / 2f);

            _player = new Player(this);
            _npc = new NPC(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _scene = new Scene(this, "MainScene");
            _background = Content.Load<Texture2D>("background1");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.R)) Initialize();

            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            _player.Update(gameTime);
            _npc.Update(gameTime);
            _player.Die();
            Camera.LookAt(_player.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin();


            _spriteBatch.Draw(_background, new Vector2(5, 5), Color.White);
            _scene.Draw(_spriteBatch, gameTime);
            _npc.Draw(_spriteBatch, gameTime);
            _player.Draw(_spriteBatch, gameTime);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
