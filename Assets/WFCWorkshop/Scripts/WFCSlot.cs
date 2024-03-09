using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace WFCWorkshop
{
    public class WFCSlot
    {

        private List<TileBase> _domain;
        private float _entropy;
        private Vector3Int _position;
        private TileBase _tile;
        private TileBase _unknown;

        public Vector3Int Position => _position;
        public float Entropy => _entropy;
        public List<TileBase> Domain => _domain;
        
        public TileBase Tile
        {
            get
            {
                if (_domain.Count == 1)
                {
                    // Solved slot
                    return _domain[0];    
                }
                else
                {
                    // unsolved slot
                    return _unknown;
                }
                
            }
        }

        public WFCSlot(Vector3Int pos, List<TileBase> startingDomain, TileBase unknownTile)
        {
            _position = pos;
            _domain = new List<TileBase>(startingDomain);

            _entropy = _domain.Count - 1;
            _unknown = unknownTile;
        }

        public void ForceCollapse()
        {
            TileBase collapsedTile = _domain[Random.Range(0, _domain.Count)];
            _domain.Clear();
            _domain.Add(collapsedTile);
            
            _entropy = 0;
        }

        public bool SetNewDomain(List<TileBase> propagatedSlotDomain)
        {
            List<TileBase> newDomain = _domain.Intersect(propagatedSlotDomain).ToList();

            if (newDomain.Count <= 0)
            {
                Debug.LogWarning("Domains not constitent !");
            }

            bool changed = _domain.Count != newDomain.Count;
            
            _domain = new List<TileBase>(newDomain);
            _entropy = _domain.Count - 1;

            return changed;

        }
        
    }
}
