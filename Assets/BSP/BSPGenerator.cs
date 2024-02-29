using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class BSPGenerator : MonoBehaviour
{

    public enum CutDirection
    {
        Horizontal,
        Vertical
    }

    [SerializeField] private Vector2 _size;
    [SerializeField] private Tilemap _dungeonMap;
    [SerializeField] private TileBase _tile;

    [SerializeField] [Range(0f, 1f)] private float _ratioMin;
    [SerializeField] [Range(0f, 1f)] private float _ratioMax;
    [SerializeField] private int _roomCountMin = 5;
    // [SerializeField] private float _roomCountMax;
    [SerializeField] private CutDirection _startDirection;
    [SerializeField] private int _minArea;
    [SerializeField] private float _shrinkRatio;


    public void Generate()
    {
        List<BoundsInt> rooms = new List<BoundsInt>();
        Queue<BSPNode> queue = new Queue<BSPNode>();
        Bounds originalBounds = new Bounds();

        Vector3 min, max;
        float hue = 0.0f;

        min.x = -1 * _size.x / 2f;
        max.x = +1 * _size.x / 2f;
        min.y = -1 * _size.y / 2f;
        max.y = +1 * _size.y / 2f;
        min.z = 0;
        max.z = 1;
        originalBounds.SetMinMax(min, max);

        queue.Enqueue(new BSPNode(originalBounds, _startDirection));

        while (queue.Count > 0 && rooms.Count < _roomCountMin)
        {
            BSPNode bspNode = queue.Dequeue();

            // Cut !!!!!!!!!
            float newRatio = Random.Range(_ratioMin, _ratioMax);
            Cut(bspNode.Bounds, bspNode.CutDirection, out var boundsA, out var boundsB, newRatio);


            // Pick if it fill the right size ------------------------------------------------
            if (boundsA.size.x * boundsA.size.y > _minArea)
            {
                queue.Enqueue(new BSPNode(boundsA, NewDirection(boundsA)));
            }
            else
            {
                boundsA.Expand(new Vector3(_shrinkRatio, _shrinkRatio, 1));
                
                BoundsInt toAdd = new BoundsInt();
                Vector3Int minInt = new Vector3Int((int)boundsA.min.x, (int)boundsA.min.y, (int)boundsA.min.z);
                Vector3Int maxInt = new Vector3Int((int)boundsA.max.x, (int)boundsA.max.y, (int)boundsA.max.z);
                toAdd.SetMinMax(minInt, maxInt);

                rooms.Add(toAdd);
            }

            if (boundsB.size.x * boundsB.size.y > _minArea)
            {
                queue.Enqueue(new BSPNode(boundsB, NewDirection(boundsB)));
            }
            else
            {
                boundsB.Expand(new Vector3(_shrinkRatio, _shrinkRatio, 1));
                
                BoundsInt toAdd = new BoundsInt();
                Vector3Int minInt = new Vector3Int((int)boundsB.min.x, (int)boundsB.min.y, (int)boundsB.min.z);
                Vector3Int maxInt = new Vector3Int((int)boundsB.max.x, (int)boundsB.max.y, (int)boundsB.max.z);
                toAdd.SetMinMax(minInt, maxInt);

                rooms.Add(toAdd);
            }

        }

        _dungeonMap.ClearAllTiles();

        foreach (BoundsInt room in rooms)
        {
            PaintMap(room, _dungeonMap, _tile, Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f));
        }

    }

    private static CutDirection NewDirection(Bounds bounds)
    {
        if (bounds.size.x > bounds.size.y)
            return CutDirection.Vertical;
        else
            return CutDirection.Horizontal;

    }

    private static CutDirection SwitchDirection(CutDirection direction)
    {

        // Switch direction each round ---------------------------------------------------------
        if (direction == CutDirection.Horizontal)
            direction = CutDirection.Vertical;
        else
            direction = CutDirection.Horizontal;

        return direction;
    }

    private void PaintMap(BoundsInt tiles, Tilemap map, TileBase tile, Color color)
    {
        foreach (Vector3Int pos in tiles.allPositionsWithin)
        {
            map.SetTile(pos, tile);
            map.SetColor(pos, color);
        }
    }

    private void Cut(Bounds inBounds, CutDirection direction, out Bounds boundsA, out Bounds boundsB, float ratio = 0.5f)
    {
        boundsA = inBounds;
        boundsB = inBounds;

        switch (direction)
        {

            case CutDirection.Horizontal:
                float middleY = inBounds.min.y + inBounds.size.y * ratio;

                boundsA.SetMinMax(inBounds.min, new Vector3(inBounds.max.x, middleY, inBounds.max.z));
                boundsB.SetMinMax(new Vector3(inBounds.min.x, middleY, inBounds.min.z), inBounds.max);

                break;

            case CutDirection.Vertical:
                float middleX = inBounds.min.x + inBounds.size.x * ratio;

                boundsA.SetMinMax(inBounds.min, new Vector3(middleX, inBounds.max.y, inBounds.max.z));
                boundsB.SetMinMax(new Vector3(middleX, inBounds.min.y, inBounds.min.z), inBounds.max);

                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

    }

}
