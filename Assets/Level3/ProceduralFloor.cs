using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Script randomly changing map tiles to one of the given tiles.
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class ProceduralFloor : MonoBehaviour
{
    [Header("Tiles from which one is randomly selected to swap a tile from a tilemap")]
    [SerializeField]
    private Tile[] Tiles;
    
    [Header("Chance for tile to be swapped")]
    [SerializeField]
    private float SwappingChance;

    private Tilemap floor;
    
    void Start()
    {
        floor = gameObject.GetComponent<Tilemap>();
        for (int i = floor.cellBounds.xMin; i < floor.cellBounds.xMax; i++)
        {
            for (int j = floor.cellBounds.yMin; j < floor.cellBounds.yMax; j++)
            {
                if (Random.value < SwappingChance)
                {   
                    floor.SetTile(new Vector3Int(i,j,0), Tiles[Mathf.FloorToInt(Random.Range(0, Tiles.Length))]);
                }
            }
        }
    }
}
