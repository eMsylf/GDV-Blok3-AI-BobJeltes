using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Lague {
    public class Node : IHeapItem<Node> {
        public bool Walkable;
        public Vector3 WorldPosition;
        public int GridX;
        public int GridY;

        public int GCost;
        public int HCost;
        public Node Parent;
        int heapIndex;

        public int FCost {
            get {
                return GCost + HCost;
            }
        }

        public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY) {
            Walkable = _walkable;
            WorldPosition = _worldPosition;
            GridX = _gridX;
            GridY = _gridY;
        }

        public int HeapIndex {
            get {
                return heapIndex;
            }
            set {
                heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare) {
            int compare = FCost.CompareTo(nodeToCompare.FCost);
            if (compare == 0) {
                compare = HCost.CompareTo(nodeToCompare.HCost);
            }
            return -compare;
        }
    }
}
