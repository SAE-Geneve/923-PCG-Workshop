using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WFCWorkshop
{
    [CustomEditor(typeof(WFCRulesAnalyzer))]

    public class WFCRulesAnalyzer_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WFCRulesAnalyzer generator = (WFCRulesAnalyzer)target;

            if (GUILayout.Button("Analyze"))
            {
                generator.Analyse();
            }

        }
    }
}
