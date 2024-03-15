using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFCWorkshop
{

    public class WFCAnalyzer : MonoBehaviour
    {
        [SerializeField] private Tilemap _map;
        [SerializeField] private WFCModuleSet _moduleSet;

        private void Start()
        {
            _map.gameObject.SetActive(false);
        }

        public void Analyze()
        {

            _moduleSet.Clear();

            // Iterate all positions where i could have tiles -------------------------------------------
            foreach (Vector3Int tilePosition in _map.cellBounds.allPositionsWithin)
            {
                TileBase rootTile = _map.GetTile(tilePosition);
                // Is there a tile here ?
                if(rootTile != null)
                {
                    // Iterate in every directions ------------------------------------------------
                    foreach (Vector3Int direction in WFCModuleSet.Directions)
                    {
                       TileBase moduleTile = _map.GetTile(tilePosition + direction);
                       // is there a tile in this direction ?
                       if (moduleTile != null)
                       {
                           _moduleSet.AddModuleNeighbour(rootTile, moduleTile, direction);
                       } 
                    }

                }
                
            }

        }
        
        
    }

}