
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameOfLifeGenerator))]
public class GameOfLifeGenerator_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameOfLifeGenerator gen = (GameOfLifeGenerator)target;
        
        if(GUILayout.Button("Generate"))
        {
            gen.Generate();
        }
        
        if(GUILayout.Button("GOL Iterate"))
        {
            gen.GameOfLifeIteration();
        }

    }


}
