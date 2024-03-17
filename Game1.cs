using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TopDownRPG
{
    public class Game1 : Game
    {
        public readonly GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch {  get; private set; }

        public static readonly int GridSize = 16;
        public readonly int TileSize;
        public EntityBuilder ObjectBuilder { get; private set; }
        public Player PlayerCharacter { get; private set; }
        public Scene CurrentScene { get; set; }

        public Game1(int screenX, int screenY)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            _graphics.PreferredBackBufferWidth = screenX;
            _graphics.PreferredBackBufferHeight = screenY;
            TileSize = screenY / GridSize;
            _graphics.IsFullScreen = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.MainMapTexture = Content.Load<Texture2D>("Maps/MainMap");
            Assets.MapEastTexture = Content.Load<Texture2D>("Maps/MapEast");
            Assets.MapNorthTexture = Content.Load<Texture2D>("Maps/MapNorth");

            Assets.PlayerTexture = Content.Load<Texture2D>("EntitySprites/player");
            Assets.MonsterTexture = Content.Load<Texture2D>("EntitySprites/monster");

            Assets.CombatText = Content.Load<SpriteFont>("combatText");

            ObjectBuilder = new EntityBuilder();
            PlayerCharacter = ObjectBuilder.PlayerCharacter;
            ObjectBuilder.BuildMaps(this);

            CurrentScene = ObjectBuilder.MainMap;

            PlayerCharacter.AtScene = (MapGridScene)CurrentScene;
            PlayerCharacter.AtScene.InteractableGrid[PlayerCharacter.ModX, PlayerCharacter.ModY] = PlayerCharacter;
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            CurrentScene.UpdateScene();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            CurrentScene.DrawScene();

            base.Draw(gameTime);
        }
    }
}
