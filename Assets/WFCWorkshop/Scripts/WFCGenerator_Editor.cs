using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WFCWorkshop
{
    [CustomEditor(typeof(WFCGenerator))]

    public class WFCGenerator_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WFCGenerator generator = (WFCGenerator)target;
            
            if (GUILayout.Button("Initiate"))
            {
                generator.Initiate();
            }
            if (GUILayout.Button("Step"))
            {
                generator.Step();
            }

        }
    }
}
