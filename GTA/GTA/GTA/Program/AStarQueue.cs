using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTA
{
    class AStarQueue
    {
        private Node[] array;
        private int length = 0;

        public AStarQueue()
        {
            array = new Node[2];
        }

        public void Enqueue(Node node)
        {
            if (array.Length == length)
                Enlarge();

            array[length] = node;
            length++;
            for (int i = length - 2 ; i >= 0; i++)
            {
                if (array[i].DistanceToGoal > node.DistanceToGoal)
                {
                    array[i + 1] = array[i];
                    array[i] = node;
                }
                if (array[i].DistanceToGoal <= node.DistanceToGoal)
                    break;
            }
        }

        public bool Contains(Node node)
        {
            return array.Any(node1 => node1 == node);
        }

        private void Enlarge()
        {
            Node[] ghostArray = array;
            array = new Node[length*2 + 1];

            for (int i = 0; i < length; i++)
                array[i] = ghostArray[i];
        }

        public Node Dequeue()
        {
            Node returnNode = array[0];
            ShiftUp();
            length--;
            return returnNode;
        }

        public int GetLength()
        {
            return length;
        }

        private void ShiftUp()
        {
            for (int i = 0; i < length; i++)
            {
                if (i == length - 1)
                    array[i] = null;
                else
                    array[i] = array[i + 1];
            }
        }
    }
}
