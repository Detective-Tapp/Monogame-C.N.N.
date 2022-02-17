using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Monogame_Cnn
{
    public class Ai : Unit
    {
        NeuralNetwork Cnet;
        public NeuralNetwork network;
        Random random;

        Vector2 Desired;
        Vector2 Velocity = new Vector2(0, 0);
        Vector2 accelleration = new Vector2(0, 0);

        float time;
        float Speed;
        float maxForce = 0.045f;

        int r=255, b=255, g=255;
        public Ai(Vector2 COORDS) : base("Agent", COORDS, new Vector2(64, 64))
        {// speed and angle as input. angle out and speed out (x/y)
            Cnet = new NeuralNetwork(new int[] { 2, 4, 5, 2 });
            network = new NeuralNetwork(new int[] { 2, 4, 5, 2});
            random = new Random();
            Coords = COORDS;
            Speed = (float)random.NextDouble() + 0.2f * 10f;
            rotation = (float)random.NextDouble() * 360f;
            Desired = Vector2.Zero;
            Cnet = network;
        }
        public override void Update()
        {// maths to vehicle
            vehicleBehaviour();
            // cnn vehicle
            updatenetwork();
            base.Update();
        }

        public override void Draw()
        {
            base.Draw();
        }
        private void updatenetwork()
        {
            time += (float)Globals.gameTime.ElapsedGameTime.TotalMilliseconds;
            if (time > 5000)
            {
                Globals.reset = true;
                Evolution();
                time = 0;
                Coords = new Vector2(Globals.screenWidth / 4, Globals.screenHeight / 4);

                r = 255; b = 255; g = 255;
            }
            
            // run the cnn feedforward.
            cnnVehicle();
           
            // bad reward code.
            RewardStructure();

            // Give colour indicator based on fitness.
            r += (int)network.fitness;
            b += (int)network.fitness;
            g += (int)network.fitness;

            Tint = new Color(r, g, b);
        }  
        private void Evolution()
        {
            if (Cnet.fitness > network.fitness)
                network = Cnet;

            Cnet = network;
            network.fitness = 0;
            network.Mutate();
        }
        private void RewardStructure()
        {
            if (Globals.GetDistance(Coords, World.player.Coords) > Globals.GetDistance(Coords + Velocity, World.player.Coords))
                network.fitness -= 0.01f;
            else
                network.fitness += 0.01f;

            if (Globals.GetDistance(Coords, World.player.Coords) < 100)
                network.fitness += 0.5f;
            else
                network.fitness -= 0.1f;

            if (Globals.GetDistance(Coords, World.player.Coords) > 500)
                network.fitness -= 2;

            if (Globals.GetDistance(Coords, World.player.Coords) < 10)
            {
                network.fitness += 3;
            }
            if (Globals.GetDistance(Coords, World.player.Coords) < 50)
            {
                network.fitness += 1;
            }

            if (Globals.GetDistance(Coords, World.player.Coords) > 200)
                network.fitness -= 1f;

            if (rotation + 0.25 <= Globals.RotateTowards(Coords, World.player.Coords) && rotation - 0.25 >= Globals.RotateTowards(Coords, World.player.Coords))
                network.AddFitness(2);
            else
                network.fitness -= 0.25f;
        }
        private void cnnVehicle()
        {
            float[] tmp = network.FeedForward(new float[] { (Globals.RotateTowards(Coords + Desired, World.player.Coords)/6.3f) * 2 -1, (Globals.GetDistance(Coords, World.player.Coords)/1000)*2 -1 });

            Vector2 steering = new Vector2(tmp[0], tmp[1]);
            steering.Normalize();
            ApplyForce(steering * maxForce);

            rotation = Globals.RotateTowards(steering * maxForce, Velocity);
            Tint = new Color(r, g, b);
        }
        private void vehicleBehaviour()
        {
            Seek();
            this.Velocity += accelleration;
            var length = Velocity.Length();
            if (length > Speed)
            {
                Velocity.Normalize();
                Velocity *= Speed;
            }
            this.Coords += Velocity;
            this.accelleration = Vector2.Zero;

        }
        public void Seek()
        {
            Vector2 Desired = World.player.Coords - this.Coords;
            Desired.Normalize();
            Desired *= Speed;
            Vector2 Steering = Desired - Velocity;
            Steering.Normalize();
            Steering *= maxForce;
            this.ApplyForce(Steering);
            rotation = Globals.RotateTowards(Steering, Velocity);
        }
        public void ApplyForce(Vector2 force)
        {
            accelleration += force;
        }

        // good for orbiting behaviour
        public Vector2 RotateVector(Vector2 tmp, float rot)
        {// for orbetting behaviour remove the added 90 degrees
            rot += (90f / 180f) * MathF.PI;

            float x = MathF.Cos(rot);
            float y = MathF.Sin(rot);
            //  for fleeing behaviour use the non negative values
            //  They are negative because screen space = a reversed axis plane
            return new Vector2(-x, -y);
        }
        public static Vector2 RotateVectorAround(Vector2 vector, Vector2 pivot, double angle)
        {
            //Get the X and Y difference
            float xDiff = vector.X - pivot.X;
            float yDiff = vector.Y - pivot.Y;

            //Rotate the vector
            float x = (float)((Math.Cos(angle) * xDiff) - (Math.Sin(angle) * yDiff) + pivot.X);
            float y = (float)((Math.Sin(angle) * xDiff) + (Math.Cos(angle) * yDiff) + pivot.Y);

            return new Vector2(x, y);
        }

    }
}
