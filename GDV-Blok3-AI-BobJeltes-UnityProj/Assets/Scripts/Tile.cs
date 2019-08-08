using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
    private int f;
    private int g;
    private int h;
    private Vector3 postition;

    public Tile(int f, int g, int h, Vector3 postition) {
        this.f = f;
        this.g = g;
        this.h = h;
        this.postition = postition;
    }

    public int FCost {
        get {
            return (g + h);
        }
    }

    public int CalculateG() {
        return g;
    }

    public int CalculateH() {
        return h;
    }
}
