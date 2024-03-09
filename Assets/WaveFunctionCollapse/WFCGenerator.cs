using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace WaveFunctionCollapse
{
    public class WFCGenerator : MonoBehaviour
    {

        [SerializeField] private Tilemap _map;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private WFCModuleSet _moduleSet;
        [SerializeField] private TileBase _undeterminedTile;
        [SerializeField] private float _autoPeriod = 0.5f;

        private List<WFCSlot> _grid = new List<WFCSlot>();

        private Coroutine _autoFill;

        private void Start()
        {
            _autoFill = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(GetBounds(_size).center, GetBounds(_size).size);
        }

        private BoundsInt GetBounds(Vector2Int size)
        {
            BoundsInt boundsInt = new BoundsInt();

            Vector3Int min = new Vector3Int(
                Mathf.FloorToInt(-1 * size.x / 2f),
                Mathf.FloorToInt(-1 * size.y / 2f),
                0
            );
            Vector3Int max = new Vector3Int(
                Mathf.CeilToInt(size.x / 2f),
                Mathf.CeilToInt(size.y / 2f),
                1
            );

            boundsInt.SetMinMax(min, max);
            return boundsInt;
        }

        public void StartAutoFill()
        {
            if(_autoFill == null)
            {
                Initiate();
                _autoFill = StartCoroutine("AutoFill");
            }
            
        }
        public void StopAutoFill()
        {
            if(_autoFill != null)
            {
                StopCoroutine(_autoFill);
                _autoFill = null;
            }
        }
        
        private IEnumerator AutoFill()
        {
            while (!Step())
            {
                if (_autoPeriod > Mathf.Epsilon)
                    yield return new WaitForSeconds(_autoPeriod);
                else
                    yield return new WaitForEndOfFrame();
            }
            
            _autoFill = null;
            
        }
        
        public void Initiate()
        {
            // Initiate the grid -----------------------------------------------
            BoundsInt gridSpace = GetBounds(_size);
            _map.ClearAllTiles();
            _grid.Clear();
            _moduleSet.ResetTileSet();

            foreach (Vector3Int pos in gridSpace.allPositionsWithin)
            {
                _grid.Add(new WFCSlot(pos, _moduleSet.Tileset, _undeterminedTile));
                _map.SetTile(pos, _undeterminedTile);
                _map.SetColor(pos, Color.gray);
            }

        }

        public bool Step()
        {
            // Find minimal entropy (but > 0) of the grid
            var collapsableSlots = _grid.Where(t => t.Entropy > 0).OrderBy(t => t.Entropy).ToList();

            if (collapsableSlots.Count > 0)
            {
                float minEntropy = collapsableSlots[0].Entropy;

                // Collapse one remaining tile among the tiles win min entropy --------------
                WFCSlot collapsedSlot = collapsableSlots.OrderBy(s => Random.value).First(s => s.Entropy == minEntropy);

                collapsedSlot.ForceCollapse();
                // Change it --------------------------------
                Debug.Log("New collapsed tile: [" + collapsedSlot.Position + "]" + collapsedSlot.Tile.name);
                _map.SetTile(collapsedSlot.Position, collapsedSlot.Tile);
                _map.SetColor(collapsedSlot.Position, Color.white);

                // Then propagate (Rules, everything, bla bla)
                if (!Propagate(collapsedSlot))
                {
                    Debug.LogWarning("Contradiction detected. Reset the collapse operation.");
                    Initiate();
                }
                
            }
            else
            {
                Debug.Log("All slots collapsed");
                return true;
            }

            return false;

        }

        private bool Propagate(WFCSlot propagatorSlot)
        {
            Stack<WFCSlot> slots = new Stack<WFCSlot>();
            List<WFCSlot> visitedSlots = new List<WFCSlot>();

            Vector3Int[] directions = new[]
            {
                Vector3Int.up,
                Vector3Int.right,
                Vector3Int.down,
                Vector3Int.left
            };

            slots.Push(propagatorSlot);

            do
            {
                WFCSlot currentSlot = slots.Pop();
                visitedSlots.Add(currentSlot);

                foreach (Vector3Int direction in directions)
                {
                    var possibleSlot = _grid.FirstOrDefault(s => s.Position == currentSlot.Position + direction && s.Entropy > 0);
                    if (possibleSlot != null && !visitedSlots.Contains(possibleSlot))
                    {
                        var possibleTiles = _moduleSet.GetTiles(currentSlot, WFCModule.GuessNeighbourhood(direction));
                        // ---------------------------------------------------------------------------------
                        Debug.Log(currentSlot.Position + " : " + possibleSlot.Position);
                        // foreach (TileBase tile in possibleTiles)
                        // {
                        //     Debug.Log(tile.name);
                        // }
                        // ---------------------------------------------------------------------------------

                        if (possibleSlot.SetNewDomain(possibleTiles))
                        {
                            Debug.Log("New Domain set at position " + possibleSlot.Position);
                            slots.Push(possibleSlot);
                        }
                        
                        // Check action depends of new entropy
                        if(possibleSlot.Entropy == -1)
                        {
                            Debug.LogWarning("Possible Contradiction. Reset the collapse operation.");
                            return false;
                        }
                        if (possibleSlot.Entropy == 0)
                        {
                            // Change it --------------------------------
                            Debug.Log("New collapsed tile: [" + possibleSlot.Position + "]" + possibleSlot.Tile.name);
                            _map.SetTile(possibleSlot.Position, possibleSlot.Tile);
                            _map.SetColor(possibleSlot.Position, Color.white);

                        }
                    }
                }


            } while (slots.Count > 0);

            return true;

        }

    }
}
