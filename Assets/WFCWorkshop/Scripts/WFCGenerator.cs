using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace WFCWorkshop
{
    public class WFCGenerator : MonoBehaviour
    {
        [Header("Generation")] [SerializeField]
        private Vector2Int _size;

        [SerializeField] private Tilemap _map;
        [SerializeField] private TileBase _unknown;
        [SerializeField] private WFCModuleSet _moduleSet;

        private List<WFCSlot> _slots = new List<WFCSlot>();

        private bool autoContinue = true;

        private void Start()
        {
            _map.gameObject.SetActive(true);
            Initiate();
        }

        private void Update()
        {
            if(autoContinue)
                autoContinue = Step();

            // if (!Step())
            //     Initiate();
            
            
        }

        

        public void Initiate()
        {
            _map.ClearAllTiles();
            _slots.Clear();
            _moduleSet.ResetTileSet();

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                   // _map.SetTile(pos, _unknown);

                    // -----------------------------------------
                    _slots.Add(new WFCSlot(pos, _moduleSet.TileSet, _unknown));

                }
            }
            
            // var starter =  _slots.OrderBy(s => Random.Range(0f, 1f)).First();
            // starter.ForceCollapse();
            
        }

        public bool Step()
        {

            List<WFCSlot> collapsableSlots = _slots.Where(s => s.Entropy > 0).OrderBy(s => s.Entropy).ToList();

            if (collapsableSlots.Count > 0)
            {
                // Observation ----------------------
                float minEntropy = collapsableSlots[0].Entropy;
                var slot = collapsableSlots.OrderBy(s => Random.Range(0f, 1f)).First(s => s.Entropy == minEntropy);
                slot.ForceCollapse();

                // Propagation
                if (!Propagate(slot))
                {
                    return false;
                }

                // Debug.Log("All slots collapsed ...");
                var paintableSlots = _slots.Where(s => s.Entropy == 0).ToList();
                foreach (var paintableSlot in paintableSlots)
                {
                    _map.SetTile(paintableSlot.Position, paintableSlot.Tile);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool Propagate(WFCSlot slot)
        {

            Stack<WFCSlot> slotsStack = new Stack<WFCSlot>();
            // List<WFCSlot> visitedSlots = new List<WFCSlot>(); // Visited were an issue
            slotsStack.Push(slot);

            while (slotsStack.Count > 0)
            {

                WFCSlot propagatedSlot = slotsStack.Pop();
                // visitedSlots.Add(propagatedSlot);

                foreach (Vector3Int direction in WFCModuleSet.Directions)
                {
                    var newSlot = _slots.FirstOrDefault(s => s.Position == direction + propagatedSlot.Position && s.Entropy > 0);
                    if (newSlot != null)
                    // if (newSlot != null && !visitedSlots.Contains(newSlot))
                    {

                        var possibleTiles = _moduleSet.PossibleTiles(propagatedSlot, direction);

                        if (newSlot.SetNewDomain(possibleTiles))
                        {
                            if (newSlot.Entropy >= 0)
                            {
                                slotsStack.Push(newSlot);
                            }
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

        public WFCSlot GetSlot(Vector3Int slotPos)
        {
            return _slots.FirstOrDefault(s => s.Position == slotPos);
        }
    }

}
