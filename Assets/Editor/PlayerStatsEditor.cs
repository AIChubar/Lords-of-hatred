using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStats))]
public class PlayerStatsEditor : Editor
{
    private FileDataHandler fileDataHandler;

    private GameData gameData;
    
    private bool changeSaving = false;
    
    
    private void OnEnable()
    {
        var dataPersistanceManagerSerializedObject =
            new SerializedObject(GameObject.FindObjectOfType(typeof(DataPersistenceManager)));

        DataPersistenceManager dataPersistenceManager = (DataPersistenceManager)dataPersistanceManagerSerializedObject.targetObject;
        fileDataHandler = dataPersistenceManager.getNewDataHandler();
        gameData = fileDataHandler.Load();
    }

    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();

        if (gameData != null)
        {
            changeSaving = EditorGUILayout.BeginToggleGroup ("Change saving", changeSaving);
            gameData.statLevels.Damage = EditorGUILayout.IntSlider("Damage Level", gameData.statLevels.Damage, 0, 100);
            gameData.statLevels.MaxHealth = EditorGUILayout.IntSlider ("Health Level", gameData.statLevels.MaxHealth, 0, 100);
            gameData.statLevels.ShootingDelay = EditorGUILayout.IntSlider ("Shooting Speed Level", gameData.statLevels.ShootingDelay, 0, 20);
            gameData.statLevels.MissileSpeed = EditorGUILayout.IntSlider ("Missile Speed Level", gameData.statLevels.MissileSpeed, 0, 20);
            gameData.statLevels.MovementSpeed = EditorGUILayout.IntSlider ("Movement Speed Level", gameData.statLevels.MovementSpeed, 0, 20);
            
            gameData.levelsProgression[0] = EditorGUILayout.IntSlider ("Level 1 Progress", gameData.levelsProgression[0], 0, 30);
            gameData.levelsProgression[1] = EditorGUILayout.IntSlider ("Level 2 Progress", gameData.levelsProgression[1], 0, 30);
            gameData.levelsProgression[2] = EditorGUILayout.IntSlider ("Level 3 Progress", gameData.levelsProgression[2], 0, 30);

            gameData.availableStatPoints = EditorGUILayout.IntSlider ("Available Stat Points", gameData.availableStatPoints, 0, 999);
            EditorGUILayout.EndToggleGroup ();
            
            fileDataHandler.Save(gameData);
        }
        else
        {
            if (GUILayout.Button("Generate saving file"))
            {
                gameData = new GameData();
            }
        }
        
    }
}
