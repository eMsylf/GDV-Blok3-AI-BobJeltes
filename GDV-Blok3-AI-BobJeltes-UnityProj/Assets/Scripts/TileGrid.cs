﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Bob.Old {
    public class TileGrid : MonoBehaviour {
        public Pathfinding pathfinding;
        public Transform StartPosition;
        public LayerMask WallMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        public float Distance;

        public List<Material> materials;
        Tile[,] grid;
        Dictionary<Tile, Transform> keyValuePairs; // doe hier nog iets mee

        int startPosX = 0;
        int startPosZ = 0;
        [Range(1, 20)]
        public int gridWidth = 12;
        [Range(1, 20)]
        public int gridHeight = 30;
        int gridHorizontalEnd;
        int gridVerticalEnd;

        public List<Tile> FinalPath;

        public List<GameObject> Outline;
        [NonSerialized]
        public bool IsDone = false;

        private void Awake() {
            ClearGrid();
        }

        private void Start() {
            if (pathfinding == null) {
                if (GetComponent<Pathfinding>() != null) {
                    pathfinding = GetComponent<Pathfinding>();
                    Debug.Log("Assigned pathfinding");
                } else {
                    Debug.Log("Missing Pathfinding component!");
                }
            }
            StartCoroutine(CreateGridAnim());

            startPosX = (int)transform.position.x;
            startPosZ = (int)transform.position.z;

            gridHorizontalEnd = startPosX + gridWidth;
            gridVerticalEnd = startPosZ + gridHeight;
        }

        public void CreateGrid() {
            for (int i = startPosX; i < gridWidth; i++) {
                for (int j = startPosZ; j < gridHeight; j++) {
                    //Debug.Log("Generating grid tile " + i + ", " + j);
                    //Vector2Int testVector = new Vector2Int(i, j);
                    //Tile currentTile = new Tile(1, 1, 1, testVector);
                    if (i == startPosX ||
                        i == gridWidth - 1 ||
                        j == startPosZ ||
                        j == gridHeight - 1) {
                        Outline.Add(PlaceBlock(pathfinding.Wall, new Vector3(i, 0, j), transform));
                        //yield return new WaitForSecondsRealtime(.1f);
                    }
                }
            }
            GridOutlineDone(true);
        }

        private IEnumerator CreateGridAnim() {
            for (int i = startPosX; i < gridWidth; i++) {
                for (int j = startPosZ; j < gridHeight; j++) {
                    //Debug.Log("Generating grid tile " + i + ", " + j);
                    //Vector2Int testVector = new Vector2Int(i, j);
                    //Tile currentTile = new Tile(1, 1, 1, testVector);
                    if (i == startPosX ||
                        i == gridWidth - 1 ||
                        j == startPosZ ||
                        j == gridHeight - 1) {
                        Outline.Add(PlaceBlock(pathfinding.Wall, new Vector3(i, 0, j), transform));
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            GridOutlineDone(true);
        }

        public void ClearGrid() {
            for (int i = 0; i < Outline.Count; i++) {
                Debug.Log("Destroy object " + i);
                DestroyImmediate(Outline[i].gameObject);
            }
            Outline.Clear();
        }

        private bool GridOutlineDone(bool isDone) {
            Debug.Log("<b>Grid outline is done: </b>" + isDone);
            IsDone = isDone; // Pass to global variable
            if (isDone) {
                return false;
            }
            return true;
        }


        public GameObject PlaceBlock(GameObject blockType, Vector3 targetPosition) {
            GameObject placedBlock = Instantiate(blockType, targetPosition, Quaternion.identity);
            return placedBlock;
        }

        public GameObject PlaceBlock(GameObject blockType, Vector3 targetPosition, Transform parent) {
            GameObject placedBlock = Instantiate(blockType, targetPosition, Quaternion.identity, parent);
            return placedBlock;
        }
    }
}