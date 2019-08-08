//Source: https://www.raywenderlich.com/3016-introduction-to-a-pathfinding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public GameObject OpenListCube;
    public GameObject ClosedListCube;
    public GameObject Wall;
    public TileGrid TileGrid;
    public float TestRange = 1f;

    private Transform currentTransform;
    Tile[,] openList;
    int[,] closedList;

    private int z;
    public int countdown = 60;
    [Range(1, 60)]
    public int PathFindSpeed = 1;
    Vector3 targetPosition;
    public Transform Goal;

    private bool endGoalReached = false;

    private LineRenderer lineRenderer;
    public Vector3[] linePositions;

    private void Start() {
        if (TileGrid == null) {
            if (GetComponent<TileGrid>() != null) {
                TileGrid = GetComponent<TileGrid>();
                Debug.Log("Assigned TileGrid");
            } else {
                Debug.Log("Missing TileGrid component!");
            }
        }
        currentTransform = TileGrid.StartPosition;
        if (GetComponent<LineRenderer>() == null) {
            Debug.Log("No line renderer");
        } else {
            lineRenderer = GetComponent<LineRenderer>();
        }

    }

    private void Update() {
        if (TileGrid.IsDone) {
            if (endGoalReached) { return; }
            if (CanPlaceNewBlock()) {
                FindPath();
                //PlaceBlock(ClosedListCube, targetPosition);
                if (endGoalReached) { return; }
            }
            //}
            //else if (Input.GetKeyDown(KeyCode.None)) {
            //    return;
            //} else {
            //    DetermineDirection();
        }

        

    }

    private bool CanPlaceNewBlock() {
        if (targetPosition == Goal.position) {
            endGoalReached = true;
            Debug.Log("<b>End goal reached</b>");
            return false;
        } else if (countdown > 0) {
            countdown -= PathFindSpeed;
            return false;
        } else {
            Debug.Log(targetPosition + " " + Goal.position);
            z += 1;
            targetPosition = transform.position + transform.right;

            //TestBlock(targetPosition);

            targetPosition = transform.position + new Vector3(0, 0, z);
            //PlaceBlock(ClosedListCube, targetPosition);
            countdown = 30;
            return true;
        }
    }

    

    private void FindPath() {
        Vector3 testLocation = transform.position + transform.right * -1;
        Vector3 testDirection = Vector3.zero;
        int a = 0;
        int b = 0;
        openList = new Tile[a, b];

        List<Vector3> neighbors = new List<Vector3>();

        for (int i = 0; i <= 4; i++) {
            switch (i) {
                case 0:
                    testDirection = transform.right * -1; // Check left side
                    break;
                case 1:
                    testDirection = transform.right; // Check right side
                    break;
                case 2:
                    testDirection = transform.forward; // Check front side
                    break;
                case 3:
                    testDirection = transform.forward * -1; // Check back side
                    break;
                default:
                    continue;
            }
            Physics.Raycast(new Ray(currentTransform.position, testDirection), out RaycastHit hitInfo, TestRange);
            testLocation = currentTransform.position + testDirection;

            neighbors.Add(testLocation);
            openList[5, 5] = new Tile(1,2,3, currentTransform.position);

            linePositions.SetValue(currentTransform.position, 0);
            linePositions.SetValue(currentTransform.position + transform.right * -1, 1);
            lineRenderer.SetPositions(linePositions);

            if (hitInfo.transform == null) { // If there's no block in the tested space
                Debug.Log("No object hit, placing block at " + testLocation);
                TileGrid.PlaceBlock(OpenListCube, testLocation, TileGrid.transform); // Place an OpenListCube in said space
            } else if (hitInfo.transform.GetComponent<Wall>() != null) { // If there's a wall in the tested space
                Debug.Log("Aah help there's a wall at " + testLocation); // Yell
            } else if (hitInfo.transform.GetComponent<OpenListCube>() != null) { // If there's an OpenListCube in the tested space
                currentTransform = hitInfo.transform;
                Debug.Log("There's an open list cube at " + testLocation);
            }
        }

        

    }


    //private void DetermineDirection() {
    //    if (!CanPlaceNewBlock()) {
    //        return;
    //    }

    //    if (Input.GetKeyDown(KeyCode.W)) {
    //        targetPosition = transform.position + new Vector3(-1, 0, 0);
    //        PlaceBlock(OpenListCube, targetPosition);
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.S)) {
    //        targetPosition = transform.position + new Vector3(1, 0, 0);
    //        PlaceBlock(OpenListCube, targetPosition);
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.A)) {
    //        targetPosition = transform.position + new Vector3(0, 0, -1);
    //        PlaceBlock(OpenListCube, targetPosition);
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.D)) {
    //        targetPosition = transform.position + new Vector3(0, 0, 1);
    //        PlaceBlock(OpenListCube, targetPosition);
    //        return;
    //    }
    //}
}
