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

        List<TileBase> _tileset = new List<TileBase>();

        // Static method

        public void SetModule(TileBase tile, TileBase neighbour)
        {
            string _rulesPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            //string rulesPathFilled = CreateSelfFolder(_rulesPath + Path.PathSeparator);

            if (AssetDatabase.IsValidFolder(_rulesPath))
            {
                Debug.Log("Creating tile module for " + tile.name + " and neighbour " + neighbour.name);

                if (Modules == null || Modules.Count <= 0)
                {
                    Debug.Log("Reset List !");
                    Modules = new List<WFCModule>();
                }

                WFCModule module = Modules.FirstOrDefault(m => m.Tile == tile);
                if (module == null)
                {
                    Debug.Log("Creating new module");
                    module = CreateInstance<WFCModule>();
                    module.Neighbours = new List<TileBase>();
                    module.Neighbours.Add(tile);
                    Modules.Add(module);

                    AssetDatabase.CreateAsset(module, _rulesPath + "/" + tile.name + "_gen.asset");
                }

                module.Tile = tile;
                module.Neighbours.Add(neighbour);
                
                AssetDatabase.SaveAssets();
                
            }
            else
            {
                Debug.LogError("Error creating folder");
            }
            

        }

        public void ResetTileset()
        {
            _tileset.Clear();
        }

        public List<TileBase> Tileset
        {
            get
            {
                if (_tileset == null || _tileset.Count <= 0)
                {
                    foreach (WFCModule module in Modules)
                    {
                        _tileset.Add(module.Tile);
                    }
                }

                return _tileset;
            }
        }

        public List<TileBase> PossibleTiles(List<TileBase> propagatedSlotDomain)
        {

            var filteredModules = Modules.Where(m => propagatedSlotDomain.Contains(m.Tile));

            HashSet<TileBase> tiles = new HashSet<TileBase>();
            foreach (WFCModule wfcModule in filteredModules)
            {
                tiles.AddRange(wfcModule.Neighbours);
            }

            return tiles.ToList();

        }
    }
}
