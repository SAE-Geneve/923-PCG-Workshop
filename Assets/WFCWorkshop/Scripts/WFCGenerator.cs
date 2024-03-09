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
        [Header("Analyse")]
        [SerializeField] private Tilemap _model;
        [SerializeField] private string _rulesPath = "RulesGenerated" ;
        [SerializeField] private WFCModuleSet _generatedModuleSet;
        
        [Header("Generation")]
        [SerializeField] private Vector2Int _size;
        [SerializeField] private Tilemap _map;
        [SerializeField] private TileBase _unknown;
        [SerializeField] private WFCModuleSet _moduleSet;

        private List<WFCSlot> _slots = new List<WFCSlot>();
        private string _rulesPathFilled = "";

        private static Vector3Int[] _directions =
        {
            Vector3Int.up,
            Vector3Int.right,
            Vector3Int.down,
            Vector3Int.left
        };

        private void Start()
        {
            Initiate();
            
        }

        private void Update()
        {
            if (!Step())
                Initiate();
        }

        public void Analyse()
        {

            //TileBase[] tiles = _model.GetTilesBlock(_model.cellBounds);

            Dictionary<TileBase, List<TileBase>> parsedTiles = new Dictionary<TileBase, List<TileBase>>();
            List<WFCModule> parsedModules = new List<WFCModule>();
            
            foreach (var pos in _model.cellBounds.allPositionsWithin)
            {
                TileBase tile = _model.GetTile(pos);  
                if (tile != null)
                {
                    Debug.Log("Master Tile : " + tile.name);
                    foreach (Vector3Int direction in _directions)
                    {
                        TileBase neighbour = _model.GetTile(pos + direction);
                        if (neighbour != null)
                        {
                            Debug.Log("Neighbour Tile : " + direction + " : "+ neighbour.name);
                            if(!parsedTiles.ContainsKey(tile))
                            {
                                parsedTiles.Add(tile, new List<TileBase>());
                                parsedTiles[tile].Add(tile);
                            }
                            parsedTiles[tile].Add(neighbour);
                            
                        }
                    }
                }
            }

            Debug.Log("Parsed Tiles ------------------------------------------------------------------");
            _generatedModuleSet.Modules = new List<WFCModule>();
            
            foreach (KeyValuePair<TileBase,List<TileBase>> parsedTile in parsedTiles)
            {
                _rulesPathFilled = WFCModuleSet.CreateSelfFolder(_rulesPath);
                if (_rulesPathFilled != "")
                {
                    WFCModule createdModule = ScriptableObject.CreateInstance<WFCModule>();
                    createdModule.Tile = parsedTile.Key;
                    createdModule.Neighbours = new List<TileBase>(parsedTile.Value);
                                    
                    AssetDatabase.CreateAsset(createdModule, _rulesPathFilled + "/" + createdModule.Tile.name + "2.asset");
                    _generatedModuleSet.Modules.Add(createdModule);
                }
                
            }

            AssetDatabase.SaveAssets();

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

                foreach (Vector3Int direction in _directions)
                {
                    var newSlot = _slots.FirstOrDefault(s => s.Position == direction + propagatedSlot.Position);
                    if (newSlot != null && !visitedSlots.Contains(newSlot))
                    {
                        
                        var possibleTiles = _moduleSet.PossibleTiles(propagatedSlot.Domain);

                        if (newSlot.SetNewDomain(possibleTiles))
                        {
                            slotsStack.Push(newSlot);
                        }

                        if (propagatedSlot.Entropy == 0)
                        {
                            // Collapsed
                            Debug.Log("collapsed tile");
                            _map.SetTile(propagatedSlot.Position, propagatedSlot.Tile);
                        }
                        if (propagatedSlot.Entropy == -1)
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
