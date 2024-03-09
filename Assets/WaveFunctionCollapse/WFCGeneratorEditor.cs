using UnityEditor;
using UnityEngine;

namespace WaveFunctionCollapse
{
    [CustomEditor(typeof(WFCGenerator))]
    public class WFCGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            WFCGenerator wfcGenerator = (WFCGenerator)target;
            
            base.OnInspectorGUI();

            if (GUILayout.Button("Initiate"))
            {
                wfcGenerator.Initiate();
            }
            
            if (GUILayout.Button("Next Step"))
            {
                wfcGenerator.Step();
            }
        }
    }
}
