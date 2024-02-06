using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 5;
    [SerializeField] int scoreValue = 50;
    [SerializeField] ParticleSystem hitFX;

    bool isDeadBoss = false;
    bool isColliding = false;

    AudioPlayer audioPlayer;
    Scorekeeper scoreKeeper;
    LevelManager levelManager;

    Animator myAnimator;
    CircleCollider2D myCollider;
    
    PlayerInput controls;
    Shooter shooter;
    PlayerStats playerStats;
    
    EnemyShooter enemyShooter;
    BossShooter bossShooter;
    BossShooterNoAttackAnim bossShooterNoAnim;
    ItemDropper itemDropper;
    EnemySpawner enemySpawner;

    void Awake() 
    {
        audioPlayer  = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<Scorekeeper>();
        levelManager = FindObjectOfType<LevelManager>();

        myAnimator = GetComponentInChildren(typeof(Animator)) as Animator;
        myCollider = GetComponent<CircleCollider2D>();
        controls = GetComponent<PlayerInput>();
        shooter = GetComponent<Shooter>();
        playerStats = FindObjectOfType<PlayerStats>();

        enemyShooter = GetComponent<EnemyShooter>();
        bossShooter = GetComponent<BossShooter>();
        bossShooterNoAnim = GetComponent<BossShooterNoAttackAnim>();

        itemDropper = GetComponent<ItemDropper>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Start() 
    {
        if (this.gameObject.tag != "Player") { return; }

        if (this.gameObject.tag == "Player" && SceneManager.GetActiveScene().buildIndex != 4)
        {
            health = playerStats.GetHealthStat();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        PowerUp powerUp = other.GetComponent<PowerUp>();
        
        if (damageDealer != null)
        {
            if (isColliding) { return; }
            isColliding = true;

            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            audioPlayer.PlayDamageClip();
            damageDealer.Hit();

            if (isPlayer)
            {
                shooter.attackLevel = 0;
                shooter.LowerAttackLevel();
            }

            StartCoroutine(ColliderReset());
        }

        if (powerUp != null && powerUp.isHealth && health < 5)
        {
            health = health + powerUp.AmountToAdd();
            Destroy(other.gameObject);
        }
        else if (powerUp != null && powerUp.isHealth && health >= 5)
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator ColliderReset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (isPlayer)
        {
            StartCoroutine(IFrames());

            if (health <= 0)
            {
                PlayerDeath();
            }
        } 
        else if (health <= 0)
        {
            if (itemDropper != null)
            {
                itemDropper.DropItem();
            }
            
            EnemyDeath();
        }
    }

    private void PlayerDeath()
    {
        shooter.enabled = false;
        myAnimator.SetTrigger("Dying");
        audioPlayer.PlayDeathClip();
        StartCoroutine(PlayerDeathCoRoutine());
    }

    private void EnemyDeath()
    {
        myAnimator.SetTrigger("Dying");
        myCollider.enabled = false;

        if (tag == "Enemy")
        {
            enemyShooter.enabled = false;
        }

        if (tag == "Boss")
        {
            bossShooter.enabled = false;
            enemySpawner.enabled = false;
            DestroyProjectiles();
            isDeadBoss = true;

            if (isDeadBoss)
            {
                levelManager.NextLevel();
            }

        }

        if (tag == "BossNoAnim")
        {
            bossShooterNoAnim.enabled = false;
            enemySpawner.enabled = false;
            DestroyProjectiles();
            isDeadBoss = true;

            if (isDeadBoss)
            {
                levelManager.NextLevel();
            }
        }

        audioPlayer.PlayDeathClip();
        scoreKeeper.ModifyScore(scoreValue);
        Destroy(gameObject, 2f);
    }

    void DestroyProjectiles()
    {
        GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject projectile in projectilesToDestroy)
        {
            Destroy(projectile.gameObject);
        }
    }

    IEnumerator IFrames()
    {
        myCollider.enabled = false;
        myAnimator.SetBool("IFrames", true);

        yield return new WaitForSeconds(2f);

        myAnimator.SetBool("IFrames", false);
        myCollider.enabled = true;
    }

    IEnumerator PlayerDeathCoRoutine()
    {
        controls.enabled = false;

        yield return new WaitForSeconds(3f);

        controls.enabled = true;
        Destroy(gameObject);
        levelManager.GameOver();
    }

    void PlayHitEffect()
    {
        if (hitFX != null)
        {
            ParticleSystem instance = Instantiate(hitFX, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }
}
