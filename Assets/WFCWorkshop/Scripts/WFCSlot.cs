using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace WFCWorkshop
{
    public class WFCSlot
    {

        private List<TileBase> _domain;
        private Vector3Int _position;
        private TileBase _tile;
        private TileBase _unknown;

        public Vector3Int Position => _position;
        public float Entropy => (_domain.Count - 1);
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
            _unknown = unknownTile;
        }

        public void ForceCollapse()
        {
            TileBase collapsedTile = _domain[Random.Range(0, _domain.Count)];
            _domain.Clear();
            _domain.Add(collapsedTile);
        }

        public bool SetNewDomain(List<TileBase> propagatedSlotDomain)
        {
            List<TileBase> newDomain = _domain.Intersect(propagatedSlotDomain).ToList();

            if (newDomain.Count <= 0)
            {
                string alertStr = _position + " : Domains not constitent !";

                foreach (TileBase tileBase in propagatedSlotDomain)
                {
                    alertStr += "\nPropagated domain : " + tileBase.name;
                }
                foreach (TileBase tileBase in _domain)
                {
                    alertStr += "\nExisting domain   : " + tileBase.name;
                }
                foreach (TileBase tileBase in newDomain)
                {
                    alertStr += "\n===> New  domain  : " + tileBase.name;
                }
                Debug.LogWarning(alertStr);
                _domain.Clear();
                return false;

            }
            else
            {
                bool changed = _domain.Count != newDomain.Count;
                _domain = new List<TileBase>(newDomain);
                return changed;
            }

        }

    }
}
