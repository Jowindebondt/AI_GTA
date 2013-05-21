using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class Graph
    {
        private List<Node> _nodes;

        public Graph()
        {
            _nodes = new List<Node>();
        }

        public void addNode(Node node)
        {
            _nodes.Add(node);
        }

        public List<Node> getNodes()
        {
            return _nodes;
        }

        public Node GetNodeFromPoint(int _x, int _y)
        {
            foreach (var node in _nodes)
            {
                if (node._p.X == _x && node._p.Y == _y)
                    return node;
            }
            return null;
        }
    }
}
