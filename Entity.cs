using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XnaGame
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Entity
    {
        public bool HasCollision { get; set; }
        public bool IsInteractable { get; set; }
        public Entity()
        {
        }
    }

    public abstract class Creature : Entity
    {
        public Creature() : base()
        {
            HasCollision = false;
        }
    }

    public class Player : Creature
    {
        public int ModX {  get; set; }
        public int ModY {  get; set; }
        public bool IsMoving { get; set; }
        public Player() : base()
        {
            ModX = 2;
            ModY = 4;
            IsMoving = false;
        }
    }

    public class Monster : Creature
    {
        public Monster() : base()
        {
            IsInteractable = true;
        }
    }

    public class Terrain : Entity
    {
        public Terrain() : base()
        {
            
        }
    }

    public class Grass : Terrain
    {
        public Grass() : base()
        {
            HasCollision = false;
        }
    }

    public class Water : Terrain
    {
        public Water() : base()
        {
            HasCollision = true;
        }
    }

    public class Rock : Terrain
    {
        public Rock() : base()
        {
            HasCollision = false;
        }
    }

    public class Brick : Terrain
    {
        public Brick() : base()
        {
            HasCollision = true;
        }
    }
}
