﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

namespace Lague {
    public class Pathfinding : MonoBehaviour {

        Grid grid;

        private void Awake() {
            grid = GetComponent<Grid>();
        }

        public Vector3[] FindPath(Vector3 startPos, Vector3 targetPos) {
            Vector3[] waypoints = new Vector3[0];

            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);

            if (startNode.Walkable && targetNode.Walkable) {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0) {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode) { // Path success
                        break;
                    }

                    foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
                        if (!neighbor.Walkable || closedSet.Contains(neighbor)) {
                            continue;
                        }

                        int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);
                        if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor)) {
                            neighbor.GCost = newMovementCostToNeighbor;
                            neighbor.HCost = GetDistance(neighbor, targetNode);
                            neighbor.Parent = currentNode;

                            if (!openSet.Contains(neighbor)) {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }
            }
            return waypoints = RetracePath(startNode, targetNode);
        }

        Vector3[] RetracePath(Node startNode, Node endNode) {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;
        }

        Vector3[] SimplifyPath(List<Node> path) {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++) {
                Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
                if (directionNew != directionOld) {
                    waypoints.Add(path[i-1].WorldPosition);
                }
                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB) {
            int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
            int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}
