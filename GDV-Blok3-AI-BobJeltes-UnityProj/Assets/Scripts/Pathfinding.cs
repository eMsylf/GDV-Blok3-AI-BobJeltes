﻿//Source: https://www.raywenderlich.com/3016-introduction-to-a-pathfinding

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public GameObject OpenListCube;
    public GameObject ClosedListCube;
    public GameObject Wall;

    private Transform currentTransform;
    private List<Tile> openList;
    private List<Tile> closedList;

    private int z;
    private int countdown = 60;
    Vector3 targetPosition;
    public Transform Goal;

    private bool endGoalReached = false;

    private void Start() {
        currentTransform = transform;
    }

    private void Update() {
        if (endGoalReached) { return; }
        if (CanPlaceNewBlock()) {
            PlaceBlock(ClosedListCube, targetPosition);
            if (endGoalReached) { return; }
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
        }
        else if (countdown > 0) {
            countdown -= 1;
            return false;
        } else {
            Debug.Log(targetPosition + " " + Goal.position);
            z += 1;
            targetPosition = transform.position + new Vector3(0, 0, z);
            //PlaceBlock(ClosedListCube, targetPosition);
            countdown = 30;
            return true;
        }
    }

    public void PlaceBlock(GameObject blockType, Vector3 targetPosition) {
        Instantiate(blockType, targetPosition, Quaternion.identity);
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