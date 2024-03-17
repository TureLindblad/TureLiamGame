using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownRPG
{
    public abstract class Entity
    {
        public bool HasCollision { get; set; }
        public bool IsInteractable { get; set; }
        public Texture2D Texture { get; set; }
        public Entity(Texture2D texture)
        {
            Texture = texture;
        }
    }

    public abstract class Creature : Entity
    {
        public int TranslateX { get; set; }
        public int TranslateY { get; set; }
        public string Name { get; }
        public Creature(Texture2D texture, string name) : base(texture)
        {
            HasCollision = false;
            Name = name;
        }
    }

    public class Player : Creature
    {
        public int ModX { get; set; }
        public int ModY { get; set; }
        public bool IsMoving { get; set; }
        public MapGridScene AtScene { get; set; }
        public Player(Texture2D texture, string name) : base(texture, name)
        {
            ModX = 2;
            ModY = 4;
            IsMoving = false;
        }

        public void Update()
        {

        }

        public void UpdateMoveAnimation()
        {
            if (IsMoving)
            {
                TranslateX = -100;
            }
            else
            {
                TranslateX = 0;
                TranslateY = 0;
            }
        }
    }

    public class Monster : Creature
    {
        public Monster(Texture2D texture, string name) : base(texture, name)
        {
            IsInteractable = true;
        }
    }
}
