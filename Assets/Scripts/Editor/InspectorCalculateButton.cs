using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Calculator))]
public class InspectorCalculateButton : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Calculate"))
        {
            Calculator calculator = (Calculator)target;
            calculator.Calculate();
        }
    }
}
