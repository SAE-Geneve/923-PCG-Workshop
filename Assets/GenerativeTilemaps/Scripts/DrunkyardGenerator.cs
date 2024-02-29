using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrunkyardGenerator : MonoBehaviour
{

    [SerializeField] private Vector2Int _startPosition = new Vector2Int(0, 0);
    [SerializeField] private Tilemap _map;
    [SerializeField] private TileBase _tile;

    [Header("Settings")] 
    [SerializeField] [Range(0, 5)] private float _lengthFactor = 1;
    [SerializeField] private int _area = 50;



    public void Generate()
    {

        Vector2Int position = _startPosition;
        HashSet<Vector2Int> positions = new HashSet<Vector2Int>();

        positions.Add(position);

        do
        {
            Vector2Int direction = GeneratorUtils.VonNeumannNeighbours[Random.Range(0, GeneratorUtils.VonNeumannNeighbours.Length)];

            int length = Mathf.CeilToInt(_lengthFactor * Random.Range(0f, 1f));

            for (int n = 0; n < length; n++)
            {
                position += direction;
                positions.Add(position);
            }
            
        } while (positions.Count < _area);

        GeneratorUtils.DrawMap(_map, _tile, positions);

    }


}
