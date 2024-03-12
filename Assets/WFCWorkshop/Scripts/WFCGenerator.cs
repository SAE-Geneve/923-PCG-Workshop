using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace WFCWorkshop
{
    public class WFCGenerator : MonoBehaviour
    {
        [Header("Generation")]
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Tilemap _map;
        [SerializeField] private TileBase _unknown;
        [SerializeField] private WFCModuleSet _moduleSet;

        private List<WFCSlot> _slots = new List<WFCSlot>();



        private void Start()
        {
            Initiate();
            
        }

        private void Update()
        {
            if (!Step())
                Initiate();
        }

        

        public void Initiate()
        {

            _map.ClearAllTiles();
            _slots.Clear();
            _moduleSet.ResetTileset();

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    _map.SetTile(pos, _unknown);

                    // -----------------------------------------
                    _slots.Add(new WFCSlot(pos, _moduleSet.Tileset, _unknown));

                }
            }

        }

        public bool Step()
        {

            List<WFCSlot> collapsableSlots = _slots.Where(s => s.Entropy != 0).OrderBy(s => s.Entropy).ToList();

            if (collapsableSlots.Count > 0)
            {
                // Observation ----------------------
                float minEntropy = collapsableSlots[0].Entropy;
                var slot = collapsableSlots.OrderBy(s => Random.Range(0f, 1f)).First(s => s.Entropy == minEntropy);

                slot.ForceCollapse();
                _map.SetTile(slot.Position, slot.Tile);

                // Propagation
                if (!Propagate(slot))
                {
                    return false;
                }

            }
            else
            {
                Debug.Log("All slots collapsed ...");
                return true;
            }

            return true;

        }

        private bool Propagate(WFCSlot slot)
        {

            Stack<WFCSlot> slotsStack = new Stack<WFCSlot>();
            List<WFCSlot> visitedSlots = new List<WFCSlot>();
            slotsStack.Push(slot);

            while (slotsStack.Count > 0)
            {

                WFCSlot propagatedSlot = slotsStack.Pop();
                visitedSlots.Add(propagatedSlot);

                foreach (Vector3Int direction in WFCSlot.Directions)
                {
                    var newSlot = _slots.FirstOrDefault(s => s.Position == direction + propagatedSlot.Position);
                    if (newSlot != null && !visitedSlots.Contains(newSlot))
                    {
                        
                        var possibleTiles = _moduleSet.PossibleTiles(propagatedSlot.Domain);

                        if (newSlot.SetNewDomain(possibleTiles))
                        {
                            slotsStack.Push(newSlot);
                        }

                        if (newSlot.Entropy == 0)
                        {
                            // Collapsed
                            Debug.Log("collapsed tile");
                            _map.SetTile(newSlot.Position, newSlot.Tile);
                        }
                        if (newSlot.Entropy == -1)
                        {
                            // Regenerate ----------------------------------
                            return false;
                        }


                    }
                }

            }

            return true;

        }

    }

}
