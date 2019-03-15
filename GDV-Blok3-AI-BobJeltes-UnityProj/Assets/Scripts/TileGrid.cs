using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileGrid : MonoBehaviour {
    public Pathfinding pathfinding;
    public Transform StartPosition;
    public LayerMask WallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float Distance;

    Tile[,] grid;
    Dictionary<Tile, Transform> keyValuePairs; // doe hier nog iets mee

    int startPosX = -1;
    int startPosZ = -15;
    int gridWidth = 12;
    int gridHeight = 30;
    int gridHorizontalEnd;
    int gridVerticalEnd;
    
    public List<Tile> FinalPath;

    private void Start() {
        StartCoroutine(CreateGrid());
        gridHorizontalEnd = startPosX + gridWidth;
        gridVerticalEnd = startPosZ + gridHeight;
    }

    private IEnumerator CreateGrid() {
        for (int i = startPosX; i < gridHorizontalEnd; i++) {
            for (int j = startPosZ; j < gridVerticalEnd; j++) {
                //Debug.Log("Generating grid tile " + i + ", " + j);
                //Vector2Int testVector = new Vector2Int(i, j);
                //Tile currentTile = new Tile(1, 1, 1, testVector);
                if (i == startPosX || 
                    i == gridHorizontalEnd -1 || 
                    j == startPosZ || 
                    j == gridVerticalEnd -1) {
                    pathfinding.PlaceBlock(pathfinding.Wall, new Vector3(i, 0, j));
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        GridOutlineDone(true);
    }

    private bool GridOutlineDone(bool isDone) {
        Debug.Log("<b>Grid outline is done: </b>" + isDone);
        if (isDone) {
            return false;
        }
        return true;
    }
}
