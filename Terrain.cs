using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TopDownRPG
{
    public class Terrain : Entity
    {

        public Terrain(Texture2D texture) : base(texture)
        {
        }
    }

    public class Grass : Terrain
    {
        public Grass(Texture2D texture) : base(texture)
        {
            HasCollision = false;
        }
    }

    public class Water : Terrain
    {
        public Water(Texture2D texture) : base(texture)
        {
            HasCollision = true;
        }
    }

    public class Sand : Terrain
    {
        public Sand(Texture2D texture) : base(texture)
        {
            HasCollision = false;
        }
    }

    public class Brick : Terrain
    {
        public Brick(Texture2D texture) : base(texture)
        {
            HasCollision = true;
        }
    }

    public class Bridge : Terrain
    {
        public Bridge(Texture2D texture) : base(texture)
        {
            HasCollision = false;
        }
    }
}
