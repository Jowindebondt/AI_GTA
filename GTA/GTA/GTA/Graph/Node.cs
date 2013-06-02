using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GTA
{
    class Node
    {
        public List<Edge> _edges { get; set; }
        public Point _p { get; set; }
        public bool drawn { get; set; }
        public int DistanceFromStart { get; set; }
        public int DistanceToGoal { get; set; }
        public int TotalCost { get; set; }
        public Node Previous { get; set; }
        public bool aStarVisited { get; set; }
        
        public Node(Point p)
        {
            _edges = new List<Edge>();
            _p = p;
            drawn = false;
            DistanceFromStart = -1;
            aStarVisited = false;
        }

        public void addEdge(Edge edge)
        {
            _edges.Add(edge);
        }
    }
}
