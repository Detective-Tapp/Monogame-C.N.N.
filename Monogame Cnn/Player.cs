using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Monogame_Cnn
{
    public class Player : Unit
    {
        public Player(Vector2 COORDS) : base("Player", COORDS, new Vector2(64,64))
        {

        }
        public override void Update()
        {
            Coords = Globals.mouse.GetScreenPos(Globals.mouse.newMouse);
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
