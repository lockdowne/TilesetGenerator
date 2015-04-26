using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TilesetGenerator.Controls
{
    public class DragTexture
    {
        public string Name { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Position { get; set; }

        public Rectangle Bounds
        {
            get { return Texture == null ? Rectangle.Empty : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); }
        }

        public bool Intersects(Vector2 position)
        {
            return Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1));
        }

        public bool Intersects(Rectangle bounds)
        {
            return Bounds.Intersects(bounds);
        }
    }
}
