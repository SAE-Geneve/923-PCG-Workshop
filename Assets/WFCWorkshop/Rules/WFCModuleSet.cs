using System.Collections;
using System.Collections.Generic;
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
