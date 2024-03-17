using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownRPG
{
    public class Button
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        private bool IsSelected { get; set; }
        private bool Clicked { get; set; }
        public Button(string name, int x, int y, int width, int height)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void Update()
        {
            if (IsSelected)
            {

            }
        }

        public void Draw(Game1 game)
        {
            //game._spriteBatch.DrawString(Assets.CombatText, "Attack 1", new Vector2(windowWidth / 2 - 300, windowHeight - 100), Color.White);
        }
    }
}
