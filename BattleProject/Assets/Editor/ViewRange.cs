using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Example))]
public class ViewRange : Editor
{
    void OnSceneGUI()
    {
        //ObjectCtrl component = (ObjectCtrl)target;
        //Handles.color = Color.white;
        //Handles.DrawWireDisc(component.transform.position, Vector3.up, 3f);
    }
}
