using UnityEngine;
using UnityEditor;

namespace Bob.Old {
    [CustomEditor(typeof(TileGrid))]
    public class TileGridEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            TileGrid tileGrid = (TileGrid)target;

            if (GUILayout.Button("Apply grid")) {
                Debug.Log("Apply grid");
                tileGrid.ClearGrid();
                tileGrid.CreateGrid();
            }
            //if (GUILayout.Button("Clear grid")) {
            //    Debug.Log("Clear grid");
            //    tileGrid.ClearGrid();
            //}
        }
    }
}