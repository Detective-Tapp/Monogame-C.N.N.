using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;


namespace Monogame_Cnn
{
    public static class Globals
    {
        public static bool reset;
        public static int screenHeight, screenWidth;

        public static GraphicsDevice graphicsDevice;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;

        public static McMouseControl mouse;
        public static GameTime gameTime;

        public static float GetDistance(Vector2 pos, Vector2 target)
        {
            return (float)Math.Sqrt(Math.Pow(pos.X - target.X, 2) + Math.Pow(pos.Y - target.Y, 2));
        }

        public static Vector2 RadialMovement(Vector2 focus, Vector2 COORDS, float speed)
        {
            float dist = Globals.GetDistance(COORDS, focus);

            if (dist <= speed)
            {
                return focus - COORDS;
            }
            else
            {
                return (focus - COORDS) * speed / dist;
            }
        }

        public static float RotateTowards(Vector2 position, Vector2 target)
        { 
            var direction = Vector2.Normalize(target - position);
            var angle = (float)Math.Acos(Vector2.Dot(direction, Vector2.UnitY));

            if (direction.X < 0) { return (float)Math.PI + angle; }
            else { return (float)Math.PI - angle; }
        }
    }
}
