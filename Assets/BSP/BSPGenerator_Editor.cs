using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BSPGenerator))]
public class BSPGenerator_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BSPGenerator gen = (BSPGenerator)target;
        
        if(GUILayout.Button("Generate"))
        {
            gen.Generate();
        }

    }


}
