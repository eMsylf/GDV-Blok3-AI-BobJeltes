using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    public bool IsWall;

    private int fCost;
    private int gCost;
    private int hCost;
    private Vector3 postition;

    public Tile(int fCost, int gCost, int hCost, Vector3 postition) {
        this.fCost = fCost;
        this.gCost = gCost;
        this.hCost = hCost;
        this.postition = postition;
    }

    public int FCost {
        get {
            return (gCost + hCost);
        }
    }

    public int CalculateG() {
        return gCost;
    }

    public int CalculateH() {
        return hCost;
    }
}
