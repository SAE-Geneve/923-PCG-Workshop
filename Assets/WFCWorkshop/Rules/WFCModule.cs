using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace WFCWorkshop
{
    [CreateAssetMenu(menuName = "WFC Workshop/Module", fileName = "module_1", order = 1)]
    public class WFCModule : ScriptableObject
    {
        [Serializable]
        public class NeighbourRule
        {
            public Vector3Int Direction;
            public List<TileBase> NeighbourTiles;
        }
        
        
        public TileBase RootTile;
        public List<NeighbourRule> NeighbourRules;

        public void AddNeighbour(TileBase tile, Vector3Int direction)
        {
            NeighbourRule newNeighbourRule = NeighbourRules.FirstOrDefault(r => r.Direction == direction);
            if (newNeighbourRule == null)
            {
                newNeighbourRule = new NeighbourRule();
                newNeighbourRule.Direction = direction;
                newNeighbourRule.NeighbourTiles = new List<TileBase>();
                NeighbourRules.Add(newNeighbourRule);
            }
            
            if(!newNeighbourRule.NeighbourTiles.Contains(tile))
            {
                newNeighbourRule.NeighbourTiles.Add(tile);
            }
            
            
        }

    }
}