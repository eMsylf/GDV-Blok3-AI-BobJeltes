//Source: https://www.raywenderlich.com/3016-introduction-to-a-pathfinding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public GameObject OpenListCube;
    public GameObject ClosedListCube;
    public GameObject Wall;
    public TileGrid TileGrid;
    public float DetectionRange = 1f;

    private Transform currentTransform;
    private List<Tile> openList;
    private List<Tile> closedList;

    private int z;
    private int countdown = 60;
    [Range(1, 60)]
    public int PathFindSpeed = 1;
    Vector3 targetPosition;
    public Transform Goal;

    private bool endGoalReached = false;

    private LineRenderer lineRenderer;
    public Vector3[] linePositions;

    private void Start() {
        currentTransform = transform;
        if (GetComponent<LineRenderer>() == null) {
            Debug.Log("No line renderer");
        } else {
            lineRenderer = GetComponent<LineRenderer>();
        }

    }

    private void Update() {
        if (TileGrid.IsDone) {
            TestBlock();
            if (endGoalReached) { return; }
            if (CanPlaceNewBlock()) {
                PlaceBlock(ClosedListCube, targetPosition);
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

    public GameObject PlaceBlock(GameObject blockType, Vector3 targetPosition) {
        GameObject placedBlock = Instantiate(blockType, targetPosition, Quaternion.identity);
        return placedBlock;
    }

    public GameObject PlaceBlock(GameObject blockType, Vector3 targetPosition, Transform parent) {
        GameObject placedBlock = Instantiate(blockType, targetPosition, Quaternion.identity, parent);
        return placedBlock;
    }

    private void TestBlock() {
        Physics.Raycast(new Ray(transform.position, transform.right * -1), out RaycastHit hitInfo, DetectionRange);

        linePositions.SetValue(transform.position, 0);
        linePositions.SetValue(transform.position + transform.right * -1, 1);

        lineRenderer.SetPositions(linePositions);

        if (hitInfo.transform == null) {
            Debug.Log("No object hit");
            return;
        }
        if (hitInfo.transform.GetComponent<Wall>() != null) {
            Debug.Log("Aah help there's a wall");
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
