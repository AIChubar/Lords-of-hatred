using Cinemachine;
using UnityEngine;

/// <summary>
/// Main manager containing frequently needed game objects.
/// </summary>
public class GameManager : MonoBehaviour
{
    public PlayerStats Character;

    public Pause PauseMenu;

    public CinemachineVirtualCamera FollowCamera;
    public static GameManager gameManager { get; private set; }

    void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else
        {
            gameManager = this;
        }
    }

    
}
