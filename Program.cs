namespace XnaGame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    internal class Program
    {
        static void Main()
        {
            Game1 game1 = new Game1(1920, 1080);
            game1.Run();
        }
    }

    public class Game1 : Game
    {
        public static readonly int GridSize = 16;
        private readonly int TileSize;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Entity[,] Grid;
        private Texture2D Pixel;
        private EntityBuilder Eb;
        private readonly Dictionary<TerrainEnum, Entity> TerrainEntities;
        private List<Entity> MonsterEntities;
        private readonly Player PlayerCharacter;

        public Game1(int screenX, int screenY)
        {
            graphics = new GraphicsDeviceManager(this);
            
            graphics.PreferredBackBufferWidth = screenX;
            graphics.PreferredBackBufferHeight = screenY;
            TileSize = screenY / GridSize - 1;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            Eb = new EntityBuilder();
            TerrainEntities = Eb.TerrainEntities;
            MonsterEntities = Eb.MonsterEntities;

            PlayerCharacter = new Player();

            Grid = new Entity[GridSize, GridSize];
            ImplementMatrix();
        }

        public void ImplementMatrix()
        {
            for(int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    Grid[i, j] = TerrainEntities[(TerrainEnum)Assets.StringMatrix[i * GridSize + j] - '0'];
                }
            }
            Grid[PlayerCharacter.ModX, PlayerCharacter.ModY] = PlayerCharacter;

            foreach (Monster monster in MonsterEntities)
            {
                Tuple<int, int> coordinates = GetValidCoordinate();
                Grid[coordinates.Item1, coordinates.Item2] = monster;
            }
        }

        private Tuple<int, int> GetValidCoordinate()
        {
            Random rnd = new Random();
            int x = rnd.Next(1, GridSize - 1);
            int y = rnd.Next(1, GridSize - 1);

            while (!(Grid[x, y] is Grass))
            {
                
                x = rnd.Next(1, GridSize - 1);
                y = rnd.Next(1, GridSize - 1);
            }

            return Tuple.Create(x, y);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData(new Color[] { Color.White });

            string[] texturePaths = { "rock.png", "brick.png", "grass1.png", "grass2.png", "grass3.png", "water.png", "player.png", "monster.png" };

            for (int i = 0; i < texturePaths.Length; i++)
            {
                string filePath = $"Content/{texturePaths[i]}";
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                switch (i)
                {
                    case 0:
                        Assets.RockTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 1:
                        Assets.BrickTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 2:
                        Assets.GrassTexture1 = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 3:
                        Assets.GrassTexture2 = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 4:
                        Assets.GrassTexture3 = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 5:
                        Assets.WaterTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 6:
                        Assets.PlayerTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                    case 7:
                        Assets.MonsterTexture = Texture2D.FromStream(GraphicsDevice, fileStream);
                        break;
                }

                fileStream.Dispose();

                Assets.BuildTextureMatrix(GridSize);
            }
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            spriteBatch.Dispose();
            Pixel.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            int moveDuration = 100;
            int dX = 0;
            int dY = 0;


            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && PlayerCharacter.ModY > 0) dY = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && PlayerCharacter.ModY < GridSize - 1)dY = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && PlayerCharacter.ModX > 0) dX = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && PlayerCharacter.ModX < GridSize - 1) dX = 1;

            Entity nextTileObject = Grid[PlayerCharacter.ModX + dX, PlayerCharacter.ModY + dY];

            if (!PlayerCharacter.IsMoving && !nextTileObject.HasCollision)
            {
                if (nextTileObject.IsInteractable)
                {
                    Event e = new Event();
                    e.LaunchEvent(PlayerCharacter, (Monster) nextTileObject);
                }
                MovePlayer(dX, dY, moveDuration);
            }

            base.Update(gameTime);
        }

        private void MovePlayer(int xMod, int yMod, int moveDuration)
        {
            Grid[PlayerCharacter.ModX, PlayerCharacter.ModY] =
                TerrainEntities[(TerrainEnum)Assets.StringMatrix[PlayerCharacter.ModX * GridSize + PlayerCharacter.ModY] - '0'];

            PlayerCharacter.ModX += xMod;
            PlayerCharacter.ModY += yMod;
            Grid[PlayerCharacter.ModX, PlayerCharacter.ModY] = PlayerCharacter;
            SetIsMovingForDuration(moveDuration);
        }

        private void SetIsMovingForDuration(int duration)
        {
            PlayerCharacter.IsMoving = true;

            Timer timer = null;
            timer = new Timer((state) =>
            {
                PlayerCharacter.IsMoving = false;
                timer.Dispose();
            }, null, duration, Timeout.Infinite);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            DrawGrid();
            base.Draw(gameTime);
        }

        private void DrawGrid()
        {
            int windowWidth = graphics.GraphicsDevice.Viewport.Width;
            int windowHeight = graphics.GraphicsDevice.Viewport.Height;
            int gridSize = (TileSize + 1) * GridSize;

            spriteBatch.Begin();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    spriteBatch.Draw(Assets.TextureMatrix[i, j], new Rectangle(
                        windowWidth / 2 + i * (TileSize) - gridSize / 2,
                        windowHeight / 2 + j * (TileSize) - gridSize / 2, 
                        TileSize, 
                        TileSize),
                        Color.White);

                    if (Grid[i, j] is Monster)
                    {
                        spriteBatch.Draw(Assets.MonsterTexture, new Rectangle(
                        windowWidth / 2 + i * (TileSize) - gridSize / 2,
                        windowHeight / 2 + j * (TileSize) - gridSize / 2,
                        TileSize,
                        TileSize),
                        Color.White);
                    }
                    else if (Grid[i, j] is Player)
                    {
                        spriteBatch.Draw(Assets.PlayerTexture, new Rectangle(
                        windowWidth / 2 + PlayerCharacter.ModX * (TileSize) - gridSize / 2,
                        windowHeight / 2 + PlayerCharacter.ModY * (TileSize) - gridSize / 2,
                        TileSize,
                        TileSize),
                        Color.White);
                    }
                }
            }

            spriteBatch.End();
        }

        private Texture2D GetRandomTexture()
        {
            Random rnd = new Random();
            switch (rnd.Next(0, 2))
            {
                case 0:
                    return Assets.GrassTexture1;
                case 1:
                    return Assets.GrassTexture2;
                case 2:
                    return Assets.GrassTexture3;
                default:
                    return Assets.GrassTexture1;
            }
        }
    }
}
