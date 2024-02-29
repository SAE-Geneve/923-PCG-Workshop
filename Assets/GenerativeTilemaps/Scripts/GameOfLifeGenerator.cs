using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GameOfLifeGenerator : MonoBehaviour
{

    [SerializeField] private Vector2Int _size;
    [SerializeField] private Tilemap _map;
    [SerializeField] private TileBase _floor;

    [Header("Perlin start")]
    [SerializeField] private Vector2 _perlinScale = new Vector2(1,1);
    [SerializeField] private Vector2 _perlinOffset = new Vector2(0,0);

    [Header("Game of life Conditions")]
    [SerializeField] private int[] _aliveConditions;
    [SerializeField] private int[] _deathConditions;
    
    private BoundsInt _startZone;

    private HashSet<Vector2Int> _tiles = new HashSet<Vector2Int>();
    
    public void Generate()
    {
        Debug.Log("Yaayahaha Generation !!!!!!");
        
        Init();
        
        //GameOfLifeIteration();
        
        GeneratorUtils.DrawMap(_map, _floor, _tiles);
        
    }

    private int NeighboursCount(Vector2Int startPosition)
    {
        int count = 0;
        foreach (Vector2Int neighbour in GeneratorUtils.MooreNeighbours)
        {
            if (_tiles.Contains(startPosition + neighbour))
                count++;
        }

        return count;

    }

    public void GameOfLifeIteration()
    {

        // Any live cell with fewer than two live neighbors dies, as if by underpopulation.
        // Any live cell with two or three live neighbors lives on to the next generation.
        // Any live cell with more than three live neighbors dies, as if by overpopulation.
        // Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
            
        HashSet<Vector2Int> aliveTiles = new HashSet<Vector2Int>(_tiles);
        
        for (int x = _startZone.xMin; x < _startZone.xMax; x++)
        {
            for (int y = _startZone.yMin; y < _startZone.yMax; y++)
            {

                Vector2Int cellPosition = new Vector2Int(x, y);
                int nbNeighbours = NeighboursCount(cellPosition);
                
                if (_tiles.Contains(cellPosition))
                {
                    // Alive cell : sould it die ?
                    if(_deathConditions.Contains(nbNeighbours))
                    {
                        // Dead -----------------------------------------------------------
                        aliveTiles.Remove(cellPosition);
                    }
                }
                else
                {
                    // Dead cell : sould it live ?
                    if (_aliveConditions.Contains(nbNeighbours))
                    {
                        aliveTiles.Add(cellPosition);
                    }
                }
            }
        }

        _tiles = new HashSet<Vector2Int>(aliveTiles);

        GeneratorUtils.DrawMap(_map, _floor, _tiles);
        
    }
    
    private void Init()
    {
        
        _tiles.Clear();

        _startZone.xMin = -1 * _size.x / 2;
        _startZone.xMax = _size.x / 2;
        _startZone.yMin = -1 * _size.y / 2;
        _startZone.yMax = _size.y / 2;
        
        for (int x = _startZone.xMin; x < _startZone.xMax; x++)
        {
            for (int y = _startZone.yMin; y < _startZone.yMax; y++)
            {
                
                float noiseCoordX = (x + _perlinOffset.x) / (float)_startZone.size.x;
                float noiseCoordY = (y + _perlinOffset.y) / (float)_startZone.size.y;
                    
                float perlinResult = Mathf.PerlinNoise(noiseCoordX * _perlinScale.x,  noiseCoordY * _perlinScale.y);

                if (perlinResult > 0.5f)
                    _tiles.Add(new Vector2Int(x, y));
                
            }
        }
    }
    
}
