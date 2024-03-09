using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WFCWorkshop
{
    [CreateAssetMenu(menuName = "WFC Workshop/Module", fileName = "module_1", order = 1)]
    public class WFCModule : ScriptableObject
    {
        public TileBase Tile;
        public List<TileBase> Neighbours;

    }
}