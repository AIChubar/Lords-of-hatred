using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script in which controls for stat upgrades are defined.
/// </summary>
public class StatUpgradeScript : MonoBehaviour
{
    [SerializeField] private Button HealthButton;
    
    [SerializeField] private Button DamageButton;

    [SerializeField] private Button MovementButton;

    [SerializeField] private Button ShootingDelayButton;
    
    [SerializeField] private Button MissileSpeedButton;
    
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private TextMeshProUGUI HealthLevel;
    
    [SerializeField] private TextMeshProUGUI DamageLevel;

    [SerializeField] private TextMeshProUGUI MovementLevel;

    [SerializeField] private TextMeshProUGUI ShootingDelayLevel;
    
    [SerializeField] private TextMeshProUGUI MissileSpeedLevel;

    [SerializeField] private TextMeshProUGUI AvailablePoints;


    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    // Start is called before the first frame update
    void Start()
    {
        HealthLevel.text = playerStats.statLevels.MaxHealth.ToString();
        DamageLevel.text = playerStats.statLevels.Damage.ToString();
        MovementLevel.text = playerStats.statLevels.MovementSpeed.ToString();
        ShootingDelayLevel.text = playerStats.statLevels.ShootingDelay.ToString();
        MissileSpeedLevel.text = playerStats.statLevels.MissileSpeed.ToString();
        AvailablePoints.text = playerStats.availableStatPoints.ToString();
        CheckPoints();
    }


    public void UpgradeHealth()
    {
        AudioManager.instance.Play(ButtonClick);
        playerStats.statLevels.MaxHealth += 1;
        int num;
        int.TryParse(HealthLevel.text, out num);
        HealthLevel.text = (num + 1).ToString();
        ChangePoints(-3);
        CheckPoints();
    }

    public void UpgradeDamage()
    {
        AudioManager.instance.Play(ButtonClick);
        playerStats.statLevels.Damage += 1;
        int num;
        int.TryParse(DamageLevel.text, out num);
        DamageLevel.text = (num + 1).ToString();
        ChangePoints(-3);
        CheckPoints();
    }
    
    public void UpgradeMovementSpeed()
    {
        AudioManager.instance.Play(ButtonClick);
        playerStats.statLevels.MovementSpeed += 1;
        int num;
        int.TryParse(MovementLevel.text, out num);
        MovementLevel.text = (num + 1).ToString();
        ChangePoints(-5);
        CheckPoints();
    }

    public void UpgradeShootingDelay()
    {
        AudioManager.instance.Play(ButtonClick);
        playerStats.statLevels.ShootingDelay += 1;
        int num;
        int.TryParse(ShootingDelayLevel.text, out num);
        ShootingDelayLevel.text = (num + 1).ToString();
        ChangePoints(-5);
        CheckPoints();
    }

    public void UpgradeMissileSpeed()
    {
        AudioManager.instance.Play(ButtonClick);
        playerStats.statLevels.MissileSpeed += 1;
        int num;
        int.TryParse(MissileSpeedLevel.text, out num);
        MissileSpeedLevel.text = (num + 1).ToString();
        ChangePoints(-5);
        CheckPoints();
    }

    public void ResetPoints()
    {
        AudioManager.instance.Play(ButtonClick);
        int returnedPoints = playerStats.statLevels.Damage*3 + playerStats.statLevels.MaxHealth*3 +
                     playerStats.statLevels.MissileSpeed*5 + playerStats.statLevels.MovementSpeed*5 + playerStats.statLevels.ShootingDelay*5;
        playerStats.statLevels = new StatLevels();
        
        ChangePoints(returnedPoints);
        HealthLevel.text = 0.ToString();
        DamageLevel.text = 0.ToString();
        MissileSpeedLevel.text = 0.ToString();
        ShootingDelayLevel.text = 0.ToString();
        MovementLevel.text = 0.ToString();
        CheckPoints();
    }

    private void ChangePoints(int amount)
    {
        playerStats.availableStatPoints += amount;
        int num;
        int.TryParse(AvailablePoints.text, out num);
        AvailablePoints.text = (num + amount).ToString();
    }

    private void CheckPoints()
    {
        if (playerStats.availableStatPoints < 5 || playerStats.statLevels.MovementSpeed >= 20)
        {
            MovementButton.interactable = false;
        }
        else
        {
            MovementButton.interactable = true;
        }
        
        if (playerStats.availableStatPoints < 5 || playerStats.statLevels.MissileSpeed >= 20)
        {
            MissileSpeedButton.interactable = false;
        }
        else
        {
            MissileSpeedButton.interactable = true;
        }
        
        if (playerStats.availableStatPoints < 5 || playerStats.statLevels.ShootingDelay >= 20)
        {
            ShootingDelayButton.interactable = false;
        }
        else
        {
            ShootingDelayButton.interactable = true;
        }
        
        if (playerStats.availableStatPoints < 3)
        {
            HealthButton.interactable = false;
            DamageButton.interactable = false;
        }
        else
        {
            HealthButton.interactable = true;
            DamageButton.interactable = true;
        }


    }
    
}
