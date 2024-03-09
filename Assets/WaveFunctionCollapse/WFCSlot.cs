using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{
    public class WFCSlot
    {
        private List<TileBase> _domain;
        private readonly TileBase _undetermined;
        
        private readonly Vector3Int _position;
        private int _entropy;

        public List<TileBase> Domain => _domain;
        public float Entropy => _entropy;
        public Vector3Int Position => _position;

        public TileBase Tile
        {
            get
            {
                if (_domain.Count == 1)
                    return _domain[0];
                else
                    return _undetermined;
            }
        }


        public WFCSlot(Vector3Int pos, List<TileBase> tileSet, TileBase undetermined)
        {
            // Setup position
            _position.x = pos.x;
            _position.y = pos.y;
            // Setup tiles datas
            _domain = new List<TileBase>(tileSet);
            _undetermined = undetermined;
            // Calculate entropy
            _entropy = _domain.Count - 1;
            
        }

        private string TilesToString(List<TileBase> tiles)
        {
            string toString = new string("");
            foreach (var tile in tiles)
            {
                toString += tile.name + "\n";
            }
            return toString;
        }

        public void ForceCollapse()
        {
            if(_domain.Count <= 0)
                return;
            
            TileBase collapsedTile = _domain[Random.Range(0, _domain.Count - 1)];
            _domain.Clear();
            
            _domain.Add(collapsedTile);
            _entropy = 0;

        }

        public bool SetNewDomain(List<TileBase> possibleTiles)
        {
            
            List<TileBase> newTiles = new List<TileBase>();
            
            newTiles = possibleTiles.Intersect(_domain).ToList();
            Debug.Log("Existing Domain : " + TilesToString(_domain));
            Debug.Log("Possibilities : " + TilesToString(possibleTiles));
            Debug.Log("Result : " + TilesToString(newTiles));
            
            if(newTiles.Count == 0)
            {
                _entropy = -1;
                return false;
            }   
            else
            {
                bool changed = newTiles.Count != _domain.Count;
                _domain = new List<TileBase>(newTiles);
                _entropy = _domain.Count - 1;
                return changed;   
            }
            
        }
    }
}
