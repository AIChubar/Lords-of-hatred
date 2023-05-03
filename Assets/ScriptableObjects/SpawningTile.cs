using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : AnimatedTile
{
    public GameObject SpawnedObject;

    private void OnDestroy()
    {
        Destroy(SpawnedObject);
    }
}
