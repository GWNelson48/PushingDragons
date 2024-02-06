using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    PlayerStats playerStats;
    Health playerHealth;
    Shooter playerShooter;

    public int healthAmount;
    public int attackAmount;
    public int bombAmount;

    void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        Instantiate(CharacterManager.instance.currentCharacter.playerPrefab, 
        transform.position, 
        Quaternion.identity);
    }

    private void Start() 
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerShooter = GameObject.FindGameObjectWithTag("Player").GetComponent<Shooter>();

        healthAmount = playerHealth.GetHealth();
        attackAmount = playerShooter.GetAttackLevel();
        bombAmount = playerShooter.GetBombAmount();

        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            SetPlayerStats();
        }
        else
        {
            GetPlayerStats();
            SetHealthAttackAndBombs();
        }
    }

    void Update() 
    {
        healthAmount = playerHealth.GetHealth();
        attackAmount = playerShooter.GetAttackLevel();
        bombAmount = playerShooter.GetBombAmount();

        SetPlayerStats();
    }

    void SetPlayerStats()
    {
        playerStats.SetHealthStat(healthAmount);
        playerStats.SetAttackAmount(attackAmount);
        playerStats.SetBombAmount(bombAmount);
    }

    void GetPlayerStats()
    {
        healthAmount = playerStats.GetHealthStat();
        attackAmount = playerStats.GetAttackAmount();
        bombAmount = playerStats.GetBombAmount();
    }

    void SetHealthAttackAndBombs()
    {
        playerHealth.SetHealth(healthAmount);
        playerShooter.SetAttackLevel(attackAmount);
        playerShooter.SetBombAmount(bombAmount);
    }
}
