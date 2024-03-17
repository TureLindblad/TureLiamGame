using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TopDownRPG
{
    public abstract class Scene
    {
        protected Game1 Game;
        public Scene(Game1 game) 
        {
            Game = game;
        }
        public abstract void UpdateScene();
        public abstract void DrawScene();
    }

    public abstract class MapGridScene : Scene
    {
        protected Texture2D MapTexture;
        protected string CollisionString;
        protected bool[,] CollisionGrid;
        public Entity[,] InteractableGrid { get; protected set; }
        public List<Entity> MonsterEntities { get; protected set; }
        public Dictionary<Tuple<char, int>, MapGridScene> ConnectingMaps { get; set; }
        public MapGridScene(Game1 game) : base(game)
        {
            CollisionGrid = new bool[Game1.GridSize, Game1.GridSize];
            InteractableGrid = new Entity[Game1.GridSize, Game1.GridSize];
            ConnectingMaps = new Dictionary<Tuple<char, int>, MapGridScene>();
        }

        protected void ImplementMatrix()
        {
            for (int i = 0; i < Game1.GridSize; i++)
            {
                for (int j = 0; j < Game1.GridSize; j++)
                {
                    CollisionGrid[i, j] = Convert.ToBoolean(CollisionString[j * Game1.GridSize + i] - '0');
                }
            }

            foreach (Monster monster in MonsterEntities.Cast<Monster>())
            {
                Tuple<int, int> coordinates = GetValidCoordinate();
                InteractableGrid[coordinates.Item1, coordinates.Item2] = monster;
            }
        }

        private Tuple<int, int> GetValidCoordinate()
        {
            Random rnd = new Random();
            int x = rnd.Next(2, Game1.GridSize - 2);
            int y = rnd.Next(2, Game1.GridSize - 2);

            while ((InteractableGrid[x, y] is not null) || CollisionGrid[x, y])
            {

                x = rnd.Next(2, Game1.GridSize - 2);
                y = rnd.Next(2, Game1.GridSize - 2);
            }

            return Tuple.Create(x, y);
        }

        public override void UpdateScene()
        {
            int moveDuration = 450;
            int dX = 0;
            int dY = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) dY = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down)) dY = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) dX = -1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) dX = 1;

            if (dX == 0 && dY == 0) return;

            int nextX = Game.PlayerCharacter.ModX + dX;
            int nextY = Game.PlayerCharacter.ModY + dY;

            if (nextX >= 0 && nextX < Game1.GridSize && nextY >= 0 && nextY < Game1.GridSize)
            {
                bool nextTerrainObject = CollisionGrid[Game.PlayerCharacter.ModX + dX, Game.PlayerCharacter.ModY + dY];
                Entity nextInteractableObject = InteractableGrid[Game.PlayerCharacter.ModX + dX, Game.PlayerCharacter.ModY + dY];

                if (!Game.PlayerCharacter.IsMoving && !nextTerrainObject)
                {
                    if (nextInteractableObject is not null && nextInteractableObject.IsInteractable)
                    {
                        Game.CurrentScene = new CombatScene(Game, nextInteractableObject);
                    }
                    MovePlayer(dX, dY, moveDuration);
                    Game.PlayerCharacter.UpdateMoveAnimation();
                }
            }
            else
            {
                try
                {
                    if (nextY < 0 || nextY >= Game1.GridSize)
                    {
                        SwitchMapAndMovePlayer('y', dY);
                    }
                    else if (nextX < 0 || nextX >= Game1.GridSize)
                    {
                        SwitchMapAndMovePlayer('x', dX);
                    }
                }
                catch { }
            }
        }

        private void SwitchMapAndMovePlayer(char modType, int movementMod)
        {
            Game.CurrentScene = ConnectingMaps[Tuple.Create(modType, movementMod)];

            Game.PlayerCharacter.AtScene.InteractableGrid[Game.PlayerCharacter.ModX, Game.PlayerCharacter.ModY] = null;

            if (modType == 'y') Game.PlayerCharacter.ModY = Math.Abs(Game.PlayerCharacter.ModY - (Game1.GridSize - 1));

            if (modType == 'x') Game.PlayerCharacter.ModX = Math.Abs(Game.PlayerCharacter.ModX - (Game1.GridSize - 1));

            Game.PlayerCharacter.AtScene = (MapGridScene)Game.CurrentScene;

            Game.PlayerCharacter.AtScene.InteractableGrid[Game.PlayerCharacter.ModX, Game.PlayerCharacter.ModY] = Game.PlayerCharacter;
        }

        private void MovePlayer(int xMod, int yMod, int moveDuration)
        {
            if (xMod != 0 && yMod != 0) moveDuration *= 2;

            InteractableGrid[Game.PlayerCharacter.ModX, Game.PlayerCharacter.ModY] = null;

            Game.PlayerCharacter.ModX += xMod;
            Game.PlayerCharacter.ModY += yMod;

            InteractableGrid[Game.PlayerCharacter.ModX, Game.PlayerCharacter.ModY] = Game.PlayerCharacter;

            
            SetIsMovingForDuration(moveDuration);
        }

        private async void SetIsMovingForDuration(int duration)
        {
            Game.PlayerCharacter.IsMoving = true;
            
            await Task.Delay(duration);

            Game.PlayerCharacter.IsMoving = false;
        }

        public override void DrawScene()
        {
            int windowWidth = Game._graphics.GraphicsDevice.Viewport.Width;
            int windowHeight = Game._graphics.GraphicsDevice.Viewport.Height;
            int gridSize = Game.TileSize * Game1.GridSize;

            Game._spriteBatch.Begin();

            for (int i = 0; i < Game1.GridSize; i++)
            {
                for (int j = 0; j < Game1.GridSize; j++)
                {
                    Rectangle sourceRect = new Rectangle(
                        i * MapTexture.Width / Game1.GridSize,
                        j * MapTexture.Height / Game1.GridSize,
                        Game.TileSize / 2,
                        Game.TileSize / 2
                    );

                    Rectangle destRect = new Rectangle(
                        windowWidth / 2 + i * Game.TileSize - gridSize / 2,
                        windowHeight / 2 + j * Game.TileSize - gridSize / 2,
                        Game.TileSize,
                        Game.TileSize
                    );

                    Game._spriteBatch.Draw(MapTexture, destRect, sourceRect, Color.White);

                    if (InteractableGrid[i, j] is Creature creature && InteractableGrid[i, j] is not null)
                    {
                        Game._spriteBatch.Draw(creature.Texture, 
                            new Rectangle(
                                windowWidth / 2 + i * Game.TileSize - gridSize / 2,
                                windowHeight / 2 + j * Game.TileSize - gridSize / 2 - Game.TileSize / 2,
                                Game.TileSize,
                                Game.TileSize),
                            Color.White);
                    }
                }
            }

            Game._spriteBatch.End();
        }
    }

    public class MainMapScene : MapGridScene
    {
        public MainMapScene(Game1 game) : base(game)
        {
            MapTexture = Assets.MainMapTexture;

            CollisionString = Assets.MainMapCollision;

            MonsterEntities = Game.ObjectBuilder.SpawnMonsters(4);

            ImplementMatrix();
        }
    }

    public class MapEastScene : MapGridScene
    {
        public MapEastScene(Game1 game) : base(game)
        {
            MapTexture = Assets.MapEastTexture;

            CollisionString = Assets.MapEastCollision;

            MonsterEntities = Game.ObjectBuilder.SpawnMonsters(3);

            ImplementMatrix();
        }
    }

    public class MapNorthScene : MapGridScene
    {
        public MapNorthScene(Game1 game) : base(game)
        {
            MapTexture = Assets.MapNorthTexture;

            CollisionString = Assets.MapNorthCollision;

            MonsterEntities = Game.ObjectBuilder.SpawnMonsters(5);

            ImplementMatrix();
        }
    }

    public class CombatScene : Scene
    {
        private readonly Monster MonsterNPC;
        public CombatScene(Game1 game, Entity nextInteractableObject) : base(game)
        {
            MonsterNPC = nextInteractableObject as Monster;
        }

        public override void UpdateScene()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) Game.CurrentScene = Game.PlayerCharacter.AtScene;
        }

        public override void DrawScene()
        {
            int windowWidth = Game._graphics.GraphicsDevice.Viewport.Width;
            int windowHeight = Game._graphics.GraphicsDevice.Viewport.Height;

            Game._spriteBatch.Begin();

            Game._spriteBatch.Draw(Game.PlayerCharacter.Texture, new Rectangle(
                (windowWidth - windowHeight) / 2 - 400,
                windowHeight / 2 - 200,
                windowHeight / 2,
                windowHeight / 2),
                Color.White);

            Game._spriteBatch.Draw(MonsterNPC.Texture, new Rectangle(
                (windowWidth - windowHeight) / 2 + windowHeight / 2,
                0,
                windowHeight / 2,
                windowHeight / 2),
                Color.White);
            
            Game._spriteBatch.DrawString(Assets.CombatText, Game.PlayerCharacter.Name, new Vector2(1000, 900), Color.White);
            Game._spriteBatch.DrawString(Assets.CombatText, "VS", new Vector2( windowWidth / 2 - 50, windowHeight / 2 - 50), Color.Red);
            Game._spriteBatch.DrawString(Assets.CombatText, MonsterNPC.Name, new Vector2(500, 50), Color.White);

            Game._spriteBatch.DrawString(Assets.CombatText, "Attack 1", new Vector2(windowWidth / 2 - 300, windowHeight - 100), Color.White);
            Game._spriteBatch.DrawString(Assets.CombatText, "Attack 2", new Vector2(windowWidth / 2, windowHeight - 100), Color.White);
            Game._spriteBatch.DrawString(Assets.CombatText, "Attack 3", new Vector2(windowWidth / 2 + 300, windowHeight - 100), Color.White);

            Game._spriteBatch.End();
        }
    }
}
