using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Cnn
{ 
    public class Object2D
    {
        public Vector2 Coords, dims;
        public Color Tint;
        public Texture2D myModel;
        // rotation in Radians
        public float rotation;

        public Object2D(string PATH, Vector2 POS, Vector2 DIMS)
        {
            myModel = Globals.content.Load<Texture2D>(PATH);
            Tint = Color.White;
            Coords = POS;
            dims = DIMS;
        }

        public virtual void Update()
        {

        }
        public void rotateTexture(float rot)
        {
            rotation = rot + rotation;
        }

        public void rotateTextureToMouse(float rot)
        {
            rotation = rot;
        }

        public virtual void Draw()
        {
            if (myModel != null)
            {
                Globals.spriteBatch.Draw(myModel, new Rectangle((int)(Coords.X), (int)(Coords.Y), (int)dims.X, (int)dims.Y), null, Tint, rotation, new Vector2(myModel.Bounds.Width / 2, myModel.Bounds.Height / 2), new SpriteEffects(), 0);
            }
        }

        public virtual void Draw(Vector2 ORIGIN)
        {
            if (myModel != null)
            {
                Globals.spriteBatch.Draw(myModel, new Rectangle((int)(Coords.X), (int)(Coords.Y), (int)dims.X, (int)dims.Y), null, Color.White, rotation, new Vector2(ORIGIN.X, ORIGIN.Y), new SpriteEffects(), 0);
            }
        }
    }
}
