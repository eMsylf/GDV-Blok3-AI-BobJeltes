using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {
    public GameObject fishPrefab;
    public static int tankSize = 5;

    static int numFish = 10;
    public static GameObject[] allFish = new GameObject[numFish];

    void Start() {
        for (int i = 0; i < numFish; i++) {
            Vector3 pos = new Vector3(
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    void Update() {

    }
}
