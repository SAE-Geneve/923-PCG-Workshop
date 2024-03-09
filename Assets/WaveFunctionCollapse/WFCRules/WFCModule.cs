using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse
{

    [CreateAssetMenu(menuName = "Wave function collapse/Create WFCModule", fileName = "WFCModule", order = 0)]
    public class WFCModule : ScriptableObject
    {
        public enum Neighbourhood
        {
            Up,
            Right,
            Down,
            Left,
            _none
        }

        public static Neighbourhood GuessNeighbourhood(Vector3Int dir)
        {
            if (dir == Vector3Int.up)
                return Neighbourhood.Up;
            else if (dir == Vector3Int.right)
                return Neighbourhood.Right;
            else if (dir == Vector3Int.down)
                return Neighbourhood.Down;
            else if (dir == Vector3Int.left)
                return Neighbourhood.Left;
            else
                return Neighbourhood._none;
        }
        
        [Serializable]
        public struct ModuleRule
        {
            public Neighbourhood Neighbourhood;
            public List<TileBase> Neighbours;
            
        }

        [SerializeField] private TileBase _tile;
        [SerializeField] private List<ModuleRule> _rules;

        public TileBase Tile => _tile;
        public List<ModuleRule> Rules => _rules;

    }
}
