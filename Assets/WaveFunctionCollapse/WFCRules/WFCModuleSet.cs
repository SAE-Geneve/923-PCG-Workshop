using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{

    [CreateAssetMenu(menuName = "Wave function collapse/Create WFCRuleset", fileName = "WFCRuleset", order = 0)]
    public class WFCModuleSet : ScriptableObject
    {
        [SerializeField] private List<WFCModule> _modules;

        private List<TileBase> _tileSet = new List<TileBase>();
        public List<TileBase> Tileset => _tileSet;

        public void ResetTileSet()
        {
            _tileSet.Clear();
            foreach (WFCModule module in _modules)
            {
                if(!_tileSet.Contains(module.Tile))
                {
                    _tileSet.Add(module.Tile);
                }
            }
        }

        public List<TileBase> GetTiles(WFCSlot currentSlot, WFCModule.Neighbourhood guessNeighbourhood)
        {
            var filteredModules = _modules
                .Where(m => currentSlot.Domain.Contains(m.Tile)
                            && m.Rules.Exists(r => r.Neighbourhood == guessNeighbourhood)
                );

            HashSet<TileBase> tiles = new HashSet<TileBase>();
            foreach (WFCModule wfcModule in filteredModules)
            {
                List<WFCModule.ModuleRule> guessedRules = wfcModule.Rules
                    .Where(r => r.Neighbourhood == guessNeighbourhood).ToList();

                foreach (var rule in guessedRules)
                {
                    tiles.AddRange(rule.Neighbours);
                }
            }

            return tiles.ToList();

        }
    }
}
