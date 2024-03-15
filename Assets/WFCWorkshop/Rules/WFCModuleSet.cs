using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using WFCWorkshop;


namespace WFCWorkshop
{
    [CreateAssetMenu(menuName = "WFC Workshop/Module set", fileName = "module_set", order = 0)]
    public class WFCModuleSet : ScriptableObject
    {
        public List<WFCModule> Modules;
        
        private List<TileBase> _tileSet = new List<TileBase>();
        
        public static Vector3Int[] Directions =
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };
        
        public void ResetTileSet()
        {
            _tileSet.Clear();
        }

        public List<TileBase> TileSet
        {
            get
            {
                if (_tileSet == null || _tileSet.Count <= 0)
                {
                    foreach (WFCModule module in Modules)
                    {
                        _tileSet.Add(module.RootTile);
                    }
                }

                return _tileSet;
            }
        }

        public List<TileBase> PossibleTiles(WFCSlot slot, Vector3Int direction)
        {
            // Result
            HashSet<TileBase> tiles = new HashSet<TileBase>();
            
            // All modules corresponding to the root tile
            var filteredModules = Modules.Where(m => slot.Domain.Contains(m.RootTile));
            
            foreach (WFCModule wfcModule in filteredModules)
            {
                // All "rules" corresponding to a direction
                List<WFCModule.NeighbourRule> rules = wfcModule.NeighbourRules.Where(r => r.Direction == direction).ToList();
                
                foreach (WFCModule.NeighbourRule rule in rules)
                {
                    tiles.AddRange(rule.NeighbourTiles);
                }
            }

            return tiles.ToList();

        }

        public void Clear()
        {
            // Clear files
            foreach (WFCModule wfcModule in Modules)
            {
                string assetPath = AssetDatabase.GetAssetPath(wfcModule);
                if(File.Exists(assetPath))
                {
                    AssetDatabase.DeleteAsset(assetPath);
                }
            }
            // Clear list
            Modules.Clear();
            
        }
        
        public void AddModuleNeighbour(TileBase rootTile, TileBase neighbourTile, Vector3Int direction)
        {
            Debug.Log("Root module tile : " + rootTile.name + " : " + neighbourTile.name);

            string modulePath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            if(modulePath != "")
            {
                WFCModule newModule = Modules.FirstOrDefault(m => m.RootTile == rootTile);
                if (newModule == null)
                {
                    // Create a fresh new asset corresponding to the root tile
                    newModule = CreateInstance<WFCModule>();
                    AssetDatabase.CreateAsset(newModule, modulePath + "/" + rootTile.name + ".asset");
                    Modules.Add(newModule);
                    
                    // Set the root tile
                    newModule.RootTile = rootTile;
                    newModule.NeighbourRules = new List<WFCModule.NeighbourRule>();
                    //newModule.AddNeighbour(rootTile, direction);
                }
                
                // add a neighbour if it does not exists already
                if(!newModule.NeighbourRules.Exists(r => r.NeighbourTiles.Contains(neighbourTile) && r.Direction == direction))
                {
                    newModule.AddNeighbour(neighbourTile, direction);
                }
                
                
                AssetDatabase.SaveAssets();
                
            }
            
        }
        
    }
}
