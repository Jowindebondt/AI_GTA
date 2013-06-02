using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    public class Edge
    {
        public Node _nextNode { get; set; }
        public int _cost { get; set; }

        public Edge(Node nextNode, int cost)
        {
            _nextNode = nextNode;
            _cost = cost;
        }

        public Edge(Node nextNode)
        {
            _nextNode = nextNode;
            _cost = 0;
        }
    }
}
