using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFCWorkshop
{
    public class WFCRulesAnalyzer : MonoBehaviour
    {
        [Header("Analyse")] [SerializeField] private Tilemap _model;
        [SerializeField] private string _rulesPath = "RulesGenerated";
        [SerializeField] private WFCModuleSet _generatedModuleSet;

        
        private string _rulesPathFilled = "";

        public void Analyse()
        {

            //TileBase[] tiles = _model.GetTilesBlock(_model.cellBounds);

            Dictionary<TileBase, List<TileBase>> parsedTiles = new Dictionary<TileBase, List<TileBase>>();
            List<WFCModule> parsedModules = new List<WFCModule>();
            
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
                            Debug.Log("Neighbour Tile : " + direction + " : "+ neighbour.name);
                            if(!parsedTiles.ContainsKey(tile))
                            {
                                parsedTiles.Add(tile, new List<TileBase>());
                                parsedTiles[tile].Add(tile);
                            }
                            parsedTiles[tile].Add(neighbour);
                            
                        }
                    }
                }
            }

            Debug.Log("Parsed Tiles ------------------------------------------------------------------");
            _generatedModuleSet.Modules = new List<WFCModule>();
            
            foreach (KeyValuePair<TileBase,List<TileBase>> parsedTile in parsedTiles)
            {
                _rulesPathFilled = WFCModuleSet.CreateSelfFolder(_rulesPath);
                if (_rulesPathFilled != "")
                {
                    WFCModule createdModule = ScriptableObject.CreateInstance<WFCModule>();
                    createdModule.Tile = parsedTile.Key;
                    createdModule.Neighbours = new List<TileBase>(parsedTile.Value);
                                    
                    AssetDatabase.CreateAsset(createdModule, _rulesPathFilled + "/" + createdModule.Tile.name + "2.asset");
                    _generatedModuleSet.Modules.Add(createdModule);
                }
                
            }

            AssetDatabase.SaveAssets();

        }
    }

}