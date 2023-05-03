using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.OnEnemyKilled += GameEvents_OnEnemyKilled;
    }

    private void GameEvents_OnEnemyKilled(KillableEnemy enemy)
    {
        if (gameObject != null && enemy.gameObject == gameObject)
        {
            Destroy(gameObject);
        }
        
    }
}
