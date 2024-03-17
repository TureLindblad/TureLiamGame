using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XnaGame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public static class Assets
    {
        //Grass = 0
        //Water = 1
        //Rock = 2
        //Brick = 3
        public static string StringMatrix =
            "3333002200000000" +
            "3333002200000000" +
            "3333002200000000" +
            "0000002200000001" +
            "2222222200000111" +
            "2222222200011100" +
            "0000002001110000" +
            "0000002111000000" +
            "0000112100000000" +
            "0001102000000000" +
            "0001100003333330" +
            "0000110003333330" +
            "0000011003333330" +
            "0000001003333330" +
            "0000001003333330" +
            "0000001100000000";

        public static Texture2D GrassTexture1;
        public static Texture2D GrassTexture2;
        public static Texture2D GrassTexture3;
        public static Texture2D WaterTexture;
        public static Texture2D RockTexture;
        public static Texture2D BrickTexture;
        public static Texture2D PlayerTexture;
        public static Texture2D MonsterTexture;


        public static Texture2D[,] TextureMatrix = new Texture2D[Game1.GridSize, Game1.GridSize];

        public static void BuildTextureMatrix(int gridSize)
        {
            Dictionary<char, Texture2D> map= new Dictionary<char, Texture2D>()
            {
                {'0', GrassTexture3 },
                {'1', WaterTexture },
                {'2', RockTexture },
                {'3', BrickTexture }
            };
            TextureMatrix = new Texture2D[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    TextureMatrix[i, j] = map[StringMatrix[i * gridSize + j]];
                }
            }
        }
        
    }
}