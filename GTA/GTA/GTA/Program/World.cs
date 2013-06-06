using System.Collections.Generic;
using System;
using System.Linq;
using GTA.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;

namespace GTA
{
    class World
    {
        private static World _instance;
        public readonly List<MovingEntity> MovingEntities;
        public readonly List<ObstacleEntity> ObstacleEntities;
        private MovingEntity thug;
        public Graph _graph;
        public bool _drawGraph;
        public bool _drawBrain;
        public Node endNode { get; set; }
        public Texture2D safehouseTex;
        private Point safeHouse;

        private World()
        {
            MovingEntities = new List<MovingEntity>();
            ObstacleEntities = new List<ObstacleEntity>();
            var rand = new Random();
            _graph = new Graph();
            _drawGraph = false;

            thug = new Thug() { Pos = new Vector2D(800, 450), _sourceY = 0, Strength = 50};
            safeHouse = new Point(982, 243);

            MovingEntities.Add(thug);

            for (int i = 0; i < 1; i++)
            {
                int next = rand.Next(0, 4);
                
                var seek = false;
                var explore = false;
                var wander = false;
                var flee = false;
                var astar = false;

                if (next == 1)
                    seek = true;
                else if (next == 2)
                    explore = true;
                else if (next == 3)
                    astar = true;
                else
                {
                    wander = true;
                    flee = true;
                }

                int citizenNr = rand.Next(1, 7);

                //---Random---//
                //var citizen = new Citizen { Pos = new Vector2D(rand.Next(100, 1500), rand.Next(100, 700)), _sourceY = citizenNr * 16, enemy = thug, Flee = flee, Wander = wander, Seek = seek, Explore = explore, AStar = astar, SafeHouse  = safeHouse, Strength = rand.Next(0,300) };
                
                //---Seek---//
                //var citizen = new Citizen { Pos = new Vector2D(rand.Next(100, 1500), rand.Next(100, 700)), _sourceY = citizenNr * 16, enemy = thug, Flee = false, Wander = false, Seek = true, Explore = false, AStar = false, SafeHouse  = safeHouse, Strength = rand.Next(0,300) };

                //---Flee & Wander---//
                //var citizen = new Citizen { Pos = new Vector2D(rand.Next(100, 1500), rand.Next(100, 700)), _sourceY = citizenNr * 16, enemy = thug, Flee = true, Wander = true, Seek = false, Explore = false, AStar = false, SafeHouse  = safeHouse, Strength = rand.Next(0,300) };

                //---Explore---//
                //var citizen = new Citizen { Pos = new Vector2D(rand.Next(100, 1500), rand.Next(100, 700)), _sourceY = citizenNr * 16, enemy = thug, Flee = false, Wander = false, Seek = false, Explore = true, AStar = false, SafeHouse  = safeHouse, Strength = rand.Next(0,300) };

                //---A*---//
                //var citizen = new Citizen { Pos = new Vector2D(rand.Next(100, 1500), rand.Next(100, 700)), _sourceY = citizenNr * 16, enemy = thug, Flee = false, Wander = false, Seek = false, Explore = false, AStar = true, SafeHouse  = safeHouse, Strength = rand.Next(0,300) };
                
                //---GDB---//
                var citizen = new Citizen { Pos = new Vector2D(rand.Next(100, 1500), rand.Next(100, 700)), _sourceY = citizenNr * 16, enemy = thug, Flee = false, Wander = false, Seek = false, Explore = false, AStar = false, SafeHouse  = safeHouse, Strength = rand.Next(0,300)};
                MovingEntities.Add(citizen);
            }

            for (int x = 0; x < 25; x++)
            {
                for (int y = 0; y < 14; y++)
                {
                    if (y == 0 || y == 13)
                    {
                        ObstacleEntities.Add(new Building { Pos = new Vector2D(x * 64, y * 64) });
                    }
                    else if(x == 0 || x == 24)
                    {
                        ObstacleEntities.Add(new Building { Pos = new Vector2D(x * 64, y * 64) });  
                    }
                    else if ((y == 2 || y == 5 || y == 8 || y == 11) && (x == 2 || x == 6 || x == 10 || x == 14 ||x == 18 || x == 22))
                    {
                        ObstacleEntities.Add(new Building { Pos = new Vector2D(x * 64, y * 64) });
                    }
                    else
                    {
                        ObstacleEntities.Add(new Pavement { Pos = new Vector2D(x * 64, y * 64) });   
                    }
                } 
            }
        }

        public static World GetInstance()
        {
            return _instance ?? (_instance = new World());
        }

        public void Load(GraphicsDevice graphicsDevice, ContentManager content)
        {
            foreach (var entity in MovingEntities)
                entity.Load(graphicsDevice, content);

            foreach (var entity in ObstacleEntities)
                entity.Load(graphicsDevice, content);

            safehouseTex = content.Load<Texture2D>("home");

            CreateGraph();
        }

        public void Update(float timeElapsed)
        {
            foreach (var entity in MovingEntities)
                entity.Update(timeElapsed);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var entity in ObstacleEntities)
                entity.Render(spriteBatch);

            if (_drawGraph)
            {
                foreach (var node in _graph.getNodes())
                {
                    Primitives2D.DrawCircle(spriteBatch, node._p.X, node._p.Y, 2, 25, Color.LightGreen);
                    foreach (var edge in node._edges)
                    {
                        if (edge._nextNode.drawn) continue;

                        Primitives2D.DrawLine(spriteBatch, node._p.X, node._p.Y, edge._nextNode._p.X,
                                              edge._nextNode._p.Y, Color.LightGreen);
                    }

                    if (node.Previous != null)
                        foreach (var edge in node._edges.Where(edge => edge._nextNode == node.Previous))
                            Primitives2D.DrawLine(spriteBatch, node._p.X, node._p.Y, edge._nextNode._p.X,
                                                  edge._nextNode._p.Y, Color.DarkRed, 3f);
                }

                if (endNode != null)
                {
                    Node currentNode = endNode;
                    while (currentNode.Previous != null)
                    {
                        Edge edge = currentNode._edges.First(node => node._nextNode == currentNode.Previous);
                        Primitives2D.DrawLine(spriteBatch, currentNode._p.X, currentNode._p.Y, edge._nextNode._p.X,
                                              edge._nextNode._p.Y, Color.DarkBlue, 3f);
                        currentNode = currentNode.Previous;
                    }
                    Primitives2D.FillRectangle(spriteBatch, endNode._p.X - 5, endNode._p.Y - 5, 10, 10, Color.DarkBlue);
                }

                foreach (var node in _graph.getNodes())
                    node.drawn = false;
            }

            foreach (var entity in MovingEntities)
                entity.Render(spriteBatch);

            spriteBatch.Draw(safehouseTex, new Vector2(safeHouse.X, safeHouse.Y), Color.White);
        }

        public void UpdateThug(Keys key)
        {
            const int speed = 5;

            Vector2D oldPos = thug.Pos;
            Vector2D tempHeading = new Vector2D(0, 0);
            
            if (key == Keys.Up)
            {
                thug.Pos = new Vector2D(thug.Pos.X, thug.Pos.Y - speed);
                tempHeading += thug.Pos - oldPos;
            }
            if (key == Keys.Down)
            {
                thug.Pos = new Vector2D(thug.Pos.X, thug.Pos.Y + speed);
                tempHeading += thug.Pos - oldPos;
            }
            if (key == Keys.Left)
            {
                thug.Pos = new Vector2D(thug.Pos.X - speed, thug.Pos.Y);
                tempHeading += thug.Pos - oldPos;
            }
            if (key == Keys.Right)
            {
                thug.Pos = new Vector2D(thug.Pos.X + speed, thug.Pos.Y);
                tempHeading += thug.Pos - oldPos;
            }

            thug.Heading = Vector2D.Vec2DNormalize(tempHeading);

        }

        public void CreateGraph()
        {
            int scale = 2;
            int columns = 26 * scale;
            int rows = 15 * scale;
            int multiplier = 64 / scale;

            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                    _graph.addNode(new Node(new Point(x * multiplier, y * multiplier)));

            for (int i = 0; i < _graph.getNodes().Count; i++)
            {
                Node currentNode = _graph.getNodes()[i];
                Node nextNode;
                if (i + rows < _graph.getNodes().Count)
                {
                    nextNode = _graph.getNodes()[i + rows];
                    currentNode.addEdge(new Edge(nextNode));
                    nextNode.addEdge(new Edge(currentNode));
                }
                if (i + rows + 1 < _graph.getNodes().Count && (i + 1)%rows != 0)
                {
                    nextNode = _graph.getNodes()[i + rows + 1];
                    currentNode.addEdge(new Edge(nextNode));
                    nextNode.addEdge(new Edge(currentNode)); 
                }
                if (i + rows - 1 < _graph.getNodes().Count && (i)%rows != 0)
                {
                    nextNode = _graph.getNodes()[i + rows - 1];
                    currentNode.addEdge(new Edge(nextNode));
                    nextNode.addEdge(new Edge(currentNode)); 
                }
                if (i + 1 < _graph.getNodes().Count && (i + 1)%rows != 0)
                {
                    nextNode = _graph.getNodes()[i + 1];
                    currentNode.addEdge(new Edge(nextNode));
                    nextNode.addEdge(new Edge(currentNode)); 
                }
            }

            foreach (var node in _graph.getNodes())
            {
                foreach (var obstacleEntity in ObstacleEntities)
                {
                    if (obstacleEntity.GetType() != typeof(Building)) continue;
                    
                    if ((node._p.X > obstacleEntity.Pos.X && node._p.X < obstacleEntity.Pos.X + 64) && 
                        (node._p.Y > obstacleEntity.Pos.Y && node._p.Y < obstacleEntity.Pos.Y + 64))
                    {
                        //Node is in a building
                        foreach (var edge in node._edges)
                        {
                            edge._nextNode._edges.Remove(edge._nextNode._edges.Find(e => e._nextNode == node));
                        }
                        node._edges.Clear();
                    }
                    else if ((node._p.X == obstacleEntity.Pos.X || node._p.X == obstacleEntity.Pos.X + 64) &&
                        (node._p.Y > obstacleEntity.Pos.Y && node._p.Y < obstacleEntity.Pos.Y + 64))
                    {
                        List<Edge> removeEdges = new List<Edge>();
                        //Node is at the left or the right side of a building
                        foreach (var edge in node._edges)
                        {
                            if ((edge._nextNode._p.X > obstacleEntity.Pos.X && edge._nextNode._p.X < obstacleEntity.Pos.X + 64) &&
                                (edge._nextNode._p.Y == obstacleEntity.Pos.Y || edge._nextNode._p.Y == obstacleEntity.Pos.Y + 64))
                            {
                                //Next node is at the top or the bottom side of a building
                                removeEdges.Add(edge);
                                edge._nextNode._edges.Remove(edge._nextNode._edges.Find(e => e._nextNode == node));
                            }
                        }

                        foreach (var removeEdge in removeEdges)
                        {
                            node._edges.Remove(removeEdge);
                        }
                    }
                }
            }
        }

        public void TagAgentsWithinViewRange(BaseGameEntity pEntity, double radius)
        {
            //  tags any entities contained that are within the
            //  radius of the single entity parameter

            //iterate through all entities checking for range
            foreach (MovingEntity curEntity in MovingEntities)
            {
                //first clear any current tag
                curEntity.IsTagged = false;

                Vector2D to = curEntity.Pos - pEntity.Pos;

                //the bounding radius of the other is taken into account by adding it 
                //to the range
                double range = radius + curEntity.Bradius;

                //if entity within range, tag for further consideration. (working in
                //distance-squared space to avoid sqrts)
                if ((curEntity != pEntity) && (to.LengthSq() < range * range))
                {
                    curEntity.IsTagged = true;
                }

            }//next entity
        }

        internal void TagObstaclesWithinViewRange(MovingEntity pEntity, double radius)
        {
            //iterate through all entities checking for range
            foreach (ObstacleEntity curEntity in ObstacleEntities)
            {
                //first clear any current tag
                curEntity.IsTagged = false;

                Vector2D to = curEntity.Pos - pEntity.Pos;

                //the bounding radius of the other is taken into account by adding it 
                //to the range
                double range = radius + curEntity.Bradius;

                //if entity within range, tag for further consideration. (working in
                //distance-squared space to avoid sqrts)
                if ((to.LengthSq() < range * range))
                {
                    if (pEntity.GetType() == typeof(Citizen) && (curEntity.Blocking == Blocking.All || curEntity.Blocking == Blocking.Person))
                        curEntity.IsTagged = true;
                }

            }//next entity
        }
    }
}
