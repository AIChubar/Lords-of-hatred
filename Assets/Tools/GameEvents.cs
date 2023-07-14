using System;
using UnityEngine;

/// <summary>
/// Manager that contains global events.
/// </summary>
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<KillableEnemy> OnEnemyKilled;

    public void EnemyKilled(KillableEnemy enemy)
    {
        if (OnEnemyKilled != null)
        {
            OnEnemyKilled(enemy);
        }
    }

}
