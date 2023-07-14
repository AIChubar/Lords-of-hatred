using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script responsible for player stats. Containing data that is going to be saved in the game.
/// </summary>
public class PlayerStats : MonoBehaviour, IDataPersistence
{
    /// <summary>
    /// Contains player stats upgrade levels.
    /// </summary>
    [HideInInspector]
    [SerializeField] public StatLevels statLevels;

    /// <summary>
    /// Player's progression on each boss Level.
    /// </summary>
    [HideInInspector] 
    [SerializeField] public List<int> levelsProgression;

    /// <summary>
    /// Player's available stat pooints.
    /// </summary>
    [HideInInspector]
    [SerializeField] public int availableStatPoints;
    
    [Header("Player initial characteristic")]
    
    [Tooltip("Initial damage")]
    [SerializeField] public CharacterStat Damage;

    [Tooltip("Initial maximum health")]
    [SerializeField] public CharacterStat MaxHealth;
    
    [Tooltip("Initial shooting delay")]
    [SerializeField] public CharacterStat ShootingDelay;

    [Tooltip("Initial missile speed")]
    [SerializeField] public CharacterStat MissileSpeed;

    [Tooltip("Initial movement speed")]
    [SerializeField] public CharacterStat MovementSpeed;

    [HideInInspector]
    public HealthSystem healthSystem;
    void Start()
    {
        MaxHealth.AddModifier(new StatModifier(0.35f*statLevels.MaxHealth, StatModType.PercentAdd));
        Damage.AddModifier(new StatModifier(0.35f*statLevels.Damage, StatModType.PercentAdd));
        MissileSpeed.AddModifier(new StatModifier(0.18f*statLevels.MissileSpeed, StatModType.PercentAdd));
        MovementSpeed.AddModifier(new StatModifier(0.12f*statLevels.MovementSpeed, StatModType.PercentAdd));
        for (int i = 0; i < statLevels.ShootingDelay; i++)
        {
            ShootingDelay.AddModifier(new StatModifier(-0.09f, StatModType.PercentMult));
        }


        healthSystem = new HealthSystem(MaxHealth.Value);
    }
    
    public void LoadData(GameData data)
    {
        availableStatPoints = data.availableStatPoints;
        statLevels = data.statLevels;
        levelsProgression = data.levelsProgression;
    }

    public void SaveData(ref GameData data)
    {
        data.availableStatPoints = availableStatPoints;
        data.statLevels = statLevels;
        data.levelsProgression = levelsProgression;
    }
}
/// <summary>
/// Class containing current stat levels.
/// </summary>
[Serializable]
public class StatLevels
{
    public int Damage;
    public int MaxHealth;
    public int ShootingDelay;
    public int MissileSpeed;
    public int MovementSpeed;

    
    public StatLevels()
    {
        Damage = 0;
        MaxHealth = 0;
        ShootingDelay = 0;
        MissileSpeed = 0;
        MovementSpeed = 0;
    }
}
