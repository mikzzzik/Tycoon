using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(RemoveColliders))]
public class ComponentDestroyerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RemoveColliders myScript = (RemoveColliders)target;
        if (GUILayout.Button("Destroy Colliders")) myScript.DestroyColliders();
    }

}