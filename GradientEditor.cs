/*==========================================
 Title:  Gradient Generator
 Author: Oskar Lindkvist
==========================================*/

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gradient))]
public class GradientEditor : Editor
{
    Gradient gradient;
    bool autoUpdate;
    bool fitCameraSize;

    void OnEnable()
    {
        gradient = (Gradient)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        fitCameraSize = EditorGUILayout.Toggle("Fit Camerasize", fitCameraSize);
        if (!fitCameraSize)
        {
            gradient.width = EditorGUILayout.FloatField("Mesh Width", gradient.width);
            gradient.height = EditorGUILayout.FloatField("Mesh Height", gradient.height);
        }
        autoUpdate = EditorGUILayout.Toggle("Auto Update Mesh", autoUpdate);
        if (autoUpdate)
            gradient.UpdateGradient(fitCameraSize);
        else if (GUILayout.Button("Update"))
        {
            gradient.UpdateGradient(fitCameraSize);
        }
    }
}
