using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Monogame_Cnn
{
    public class World
    {
        List<Ai> Agents = new List<Ai>();
        public static Player player;

        float agentCount = 50;
        public World()
        {
            player = new Player(new Vector2(Globals.screenWidth/2, Globals.screenHeight/2));
            
            for (int i = 0; i < agentCount; i++)
            {
                Agents.Add(new Ai(new Vector2(Globals.screenWidth / 4, Globals.screenHeight / 4)));
            }
        }
        public void Update(GameTime time)
        {
            NeuralNetwork[] networks = new NeuralNetwork[5];
            bool duplicate = true;

            player.Update();

            for (int i = 0; i < agentCount; i++)
            {
                Agents[i].Update();
                networks[(int)(i / 10)] = Agents[i].network;
            }

            if (Globals.reset)
            {// get the top 5 brains.
                for (int k = 0; k < networks.Length; k++)
                {
                    foreach (Ai i in Agents)
                    {
                        foreach (Ai j in Agents)
                        {
                            if (i.network.fitness > j.network.fitness)
                            {
                                duplicate = true;
                                foreach (var l in networks)
                                {
                                    if (l == i.network)
                                        break;
                                    else
                                        duplicate = false;
                                }
                                if (duplicate == false)
                                {
                                    networks[k] = i.network;
                                }
                            }
                        }
                    }
                }

                // insert "better" brains.
                for (int i = 0; i < 5; i++)
                {
                    for (int j = i * 10; j < (i * 10) + 10; j++)
                    {
                        Agents[j].network = networks[i];
                    }
                }
                Globals.reset = false;
            }
        }

        public void Draw()
        {
            player.Draw();
            for (int i = 0; i < agentCount; i++)
            {
                Agents[i].Draw();
            }
        }
    }
}
