using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap _map;
    [SerializeField] private TileBase _grass;
    [SerializeField] private TileBase _rock;

    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2 _perlinScale = new Vector2(1,1);
    [SerializeField] private Vector2 _perlinOffset = new Vector2(0,0);
    [SerializeField] private Vector2Int _center;


    private void Update()
    {
        Generate();
    }

    public void Generate()
    {
        
        _map.ClearAllTiles();
        
        for (int x = 0; x <_size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                
                float noiseCoordX = (x + _perlinOffset.x) / (float)_size.x;
                float noiseCoordY = (y + _perlinOffset.y) / (float)_size.y;
                    
                float perlinResult = Mathf.PerlinNoise(noiseCoordX * _perlinScale.x,  noiseCoordY * _perlinScale.y);
                
                //_map.SetColor(new Vector3Int(_center.x + x - _size.x / 2, _center.y + y - _size.y / 2), new Color(perlinResult, perlinResult, perlinResult));
                
                Debug.Log(perlinResult);
                
                if(perlinResult > 0.5f)
                    _map.SetTile( new Vector3Int(_center.x + x - _size.x / 2, _center.y + y - _size.y / 2), _grass);
                    else
                    _map.SetTile( new Vector3Int(_center.x + x - _size.x / 2, _center.y + y - _size.y / 2), _rock);
                
                _map.SetColor(
                    new Vector3Int(_center.x + x - _size.x / 2, _center.y + y - _size.y / 2), 
                    new Color(perlinResult, perlinResult, perlinResult)
                    );

            }
        }
        
    }

}
