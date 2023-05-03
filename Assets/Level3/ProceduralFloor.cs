using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralFloor : MonoBehaviour
{

    public Tile[] Tiles;

    private Tilemap Floor;
    // Start is called before the first frame update
    void Start()
    {
        Floor = gameObject.GetComponent<Tilemap>();
        for (int i = Floor.cellBounds.xMin; i < Floor.cellBounds.xMax; i++)
        {
            for (int j = Floor.cellBounds.yMin; j < Floor.cellBounds.yMax; j++)
            {
                if (Random.value < 0.3f)
                {   
                    Floor.SetTile(new Vector3Int(i,j,0), Tiles[Mathf.FloorToInt(Random.Range(0, Tiles.Length))]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
