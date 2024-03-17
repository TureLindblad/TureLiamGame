using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownRPG
{
    using Microsoft.Xna.Framework.Graphics;

    public static class Assets
    {
        public static Texture2D MainMapTexture;
        public static Texture2D MapEastTexture;
        public static Texture2D MapNorthTexture;

        public static Texture2D PlayerTexture;
        public static Texture2D MonsterTexture;

        public static SpriteFont CombatText;

        public static string[] MonsterNames = { "Goblin", "Harold the Great", "Brute Force", "Green Lantern" };

        //0 = no collision
        //1 = has collision
        public static string MainMapCollision =
            "0000000000001100" +
            "0000000000001100" +
            "0000000000011100" +
            "0000000000000000" +
            "0000000000000000" +
            "0000000000011000" +
            "0001110000111000" +
            "0001110000110000" +
            "0001110000110000" +
            "0001110000110000" +
            "0000000001110000" +
            "0000000001100000" +
            "0000000001100000" +
            "0000000001100000" +
            "0000000001100000" +
            "0000000001100000";

        public static string MapEastCollision =
            "0000000000000000" +
            "0000000000000000" +
            "0000000000000000" +
            "0000000000000000" +
            "0000000000000000" +
            "0000000000000000" +
            "1110000000000000" +
            "0010000000000000" +
            "0010000000000000" +
            "0010000000000000" +
            "0010000000000000" +
            "0010000011111100" +
            "0010000011111100" +
            "0010000000000000" +
            "0010000000000000" +
            "0010000000000000";

        public static string MapNorthCollision =
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100" +
            "0000000000001100";
    }
}