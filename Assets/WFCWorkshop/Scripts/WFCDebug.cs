using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WFCWorkshop;

public class WFCDebug : MonoBehaviour
{
    [SerializeField] private Tilemap _debugMap;
    [SerializeField] private WFCGenerator _generator;
    
    // Click to se informations on map and tiles
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D physHit2D = Physics2D.Raycast(mouseRay.origin, mouseRay.direction, Camera.main.farClipPlane);
            if (physHit2D.collider != null)
            {
                Vector3Int slotPos = _debugMap.WorldToCell(physHit2D.point);
                WFCSlot slot = _generator.GetSlot(slotPos);
                
                Debug.Log("Slot --------- " + slot.Tile.name);
                Debug.Log("Entropy : " + slot.Entropy);
                Debug.Log("Position : " + slot.Position);

                int i = 0;
                foreach (TileBase domainTile in slot.Domain)
                {
                    Debug.Log("Domain [" + ++i + "] : " + domainTile.name);
                }
                
            }
        }
    }
}
