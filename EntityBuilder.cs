using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownRPG
{
    public class EntityBuilder
    {
        public MainMapScene MainMap {  get; private set; }
        public MapEastScene MapEast {  get; private set; }
        public MapNorthScene MapNorth {  get; private set; }

        public Player PlayerCharacter;

        public EntityBuilder()
        {
            PlayerCharacter = new Player(Assets.PlayerTexture, "CockSlayer3000");
        }

        public void BuildMaps(Game1 game)
        {
            MainMap = new MainMapScene(game);
            MapEast = new MapEastScene(game);
            MapNorth = new MapNorthScene(game);

            MainMap.ConnectingMaps[Tuple.Create('y', -1)] = MapNorth;
            MainMap.ConnectingMaps[Tuple.Create('x', 1)] = MapEast;

            MapEast.ConnectingMaps[Tuple.Create('x', -1)] = MainMap;

            MapNorth.ConnectingMaps[Tuple.Create('y', 1)] = MainMap;
        }

        public List<Entity> SpawnMonsters(int amount)
        {
            List<Entity> monsterEntities = new List<Entity>();
            Random rnd = new Random();

            for (int i = 0; i < amount; i++)
            {
                monsterEntities.Add(new Monster(Assets.MonsterTexture, Assets.MonsterNames[rnd.Next(Assets.MonsterNames.Length)]));
            }

            return monsterEntities;
        }
    }
}
