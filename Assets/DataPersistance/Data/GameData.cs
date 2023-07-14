using System.Collections.Generic;

/// <summary>
/// Class containing all data that needs to be saved.
/// </summary>
[System.Serializable]
public class GameData
{
    public StatLevels statLevels;
    public List<int> levelsProgression;
    public int availableStatPoints;
    public GameData()
    {
        statLevels = new StatLevels();
        levelsProgression = new List<int>{0, 0, 0};
        availableStatPoints = 0;
    }
}
