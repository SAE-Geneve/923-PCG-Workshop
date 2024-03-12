using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFCWorkshop
{
    public class WFCRulesAnalyzer : MonoBehaviour
    {
        [Header("Analyse")] [SerializeField] private Tilemap _model;
        [SerializeField] private WFCModuleSet _generatedModuleSet;

        private string _rulesPathFilled = "";

        public void Analyse()
        {

            _generatedModuleSet.Modules = new List<WFCModule>();
            
            foreach (var pos in _model.cellBounds.allPositionsWithin)
            {
                TileBase tile = _model.GetTile(pos);  
                if (tile != null)
                {
                    Debug.Log("Master Tile : " + tile.name);
                    foreach (Vector3Int direction in WFCSlot.Directions)
                    {
                        TileBase neighbour = _model.GetTile(pos + direction);
                        if (neighbour != null)
                        {
                            _generatedModuleSet.SetModule(tile, neighbour);
                        }
                    }
                }
            }

        }
    }

}