using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XnaGame
{
    public enum TerrainEnum
    {
        Grass, Water, Rock, Brick
    }
    public class EntityBuilder
    {
        public Dictionary<TerrainEnum, Entity> TerrainEntities;
        public List<Entity> MonsterEntities;

        public EntityBuilder()
        {
            BuildTerrain();
            SpawnMonsters(4);
        }

        private void BuildTerrain()
        {
            Grass GrassObject = new Grass();
            Water WaterObject = new Water();
            Rock RockObject = new Rock();
            Brick BrickObject = new Brick();

            TerrainEntities = new Dictionary<TerrainEnum, Entity>()
            {
                { TerrainEnum.Grass, GrassObject },
                { TerrainEnum.Water, WaterObject },
                { TerrainEnum.Rock, RockObject },
                { TerrainEnum.Brick, BrickObject }
            };
        }

        private void SpawnMonsters(int amount)
        {
            MonsterEntities = new List<Entity>();

            for (int i = 0; i < amount; i++)
            {
                MonsterEntities.Add(new Monster());
            }
        }
    }
}
