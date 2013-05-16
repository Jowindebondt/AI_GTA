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

        public Node(Point p)
        {
            _edges = new List<Edge>();
            _p = p;
        }

        public void addEdge(Edge edge)
        {
            _edges.Add(edge);
        }
    }
}
