using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    static PlayerStats instance;
    Health playerHealth;
    Shooter playerShooter;

    public int healthAmount;
    public int attackAmount;
    public int bombAmount;

    public int retryLevel;

    private void Awake() 
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetHealthStat()
    {
        return healthAmount;
    }

    public void SetHealthStat(int value)
    {
        healthAmount = value;
    }

    public int GetAttackAmount()
    {
        return attackAmount;
    }

    public void SetAttackAmount(int value)
    {
        attackAmount = value;
    }

    public int GetBombAmount()
    {
        return bombAmount;
    }

    public void SetBombAmount(int value)
    {
        bombAmount = value;
    }

    public int GetRetryLevel()
    {
        return retryLevel;
    }

    public void SetRetryLevel(int value)
    {
        retryLevel = value;
    }
}
