using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shooter : MonoBehaviour
{
    [Header("Projectiles")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 6f;
    [SerializeField] float projectileLifetime = 8f;
    [SerializeField] float firingRate = 0.5f;
    public int attackLevel = 0;

    [Header("Bombs")]
    [SerializeField] GameObject bombEffectPrefab;
    public int bombAmount = 3;
    public int bombDamage = 20;

    [HideInInspector] public bool isFiring;
    [HideInInspector] public bool useBomb;
    
    Gun[] guns;
    
    Coroutine firingCoroutine;
    PlayerStats playerStats;
    AudioPlayer audioPlayer;

    void Awake() 
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Start() 
    {
        guns = transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            gun.isActive = true;
            if (gun.attackLevelRequirement != 0)
            {
                gun.isActive = false;
                gun.gameObject.SetActive(false);
            }
        }

        if (SceneManager.GetActiveScene().buildIndex != 4)
        {
            attackLevel = playerStats.GetAttackAmount();
            bombAmount = playerStats.GetBombAmount();
            GunCheck();
        }
    }

    void Update() 
    {
        foreach (Gun gun in guns)
        {
            if (gun.isActive == true)
            {
                Fire();
            }
        }

        if (useBomb && bombAmount > 0)
        {
            StartCoroutine(BombEffectCoroutine());
        }
    }

    public void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    public IEnumerator FireContinuously()
    {
        while (true)
        {
            foreach (Gun gun in guns)
            {
                if (gun.isActive)
                {
                    GameObject shot = Instantiate(projectilePrefab, gun.transform.position, Quaternion.identity);
                    Vector2 shotDirection = (gun.transform.localRotation * Vector2.up).normalized;
                    Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();

                    if (rb != null)
                    {
                        rb.velocity = shotDirection * projectileSpeed;
                    }

                    audioPlayer.PlayShootingClip();
                    Destroy(shot, projectileLifetime);
                }
            }
            yield return new WaitForSeconds(firingRate);
        }
    }

    void BombAttack()
    {
        bombAmount--;
        useBomb = false;

        Health[] enemiesHealth = FindObjectsOfType<Health>();

        foreach (Health enemyHealth in enemiesHealth)
        {
            if (enemyHealth.gameObject.layer == LayerMask.NameToLayer("Enemy") && enemyHealth != null)
            {
                enemyHealth.TakeDamage(bombDamage);
            }
        }
        
        DestroyProjectiles();
    }

    void DestroyProjectiles()
    {
        GameObject[] projectilesToDestroy = GameObject.FindGameObjectsWithTag("Projectile");

        foreach (GameObject projectile in projectilesToDestroy)
        {
            Destroy(projectile.gameObject);
        }
    }

    IEnumerator BombEffectCoroutine()
    {
        while (useBomb)
        {
            GameObject bomb = Instantiate(bombEffectPrefab, bombEffectPrefab.transform.position, Quaternion.identity);
            Vector2 shotDirection = (bomb.transform.position * (Vector2.up * -1)).normalized;
            Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = shotDirection * 25f;
            }

            BombAttack();

            Destroy(bomb, 2f);
        }

        yield return new WaitForSeconds(1f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RaiseAttackLevel(other);
        RaiseBombAmount(other);
    }

    private void RaiseAttackLevel(Collider2D other)
    {
        PowerUp powerUp = other.GetComponent<PowerUp>();

        if (powerUp != null && powerUp.isAttack)
        {
            attackLevel = attackLevel + powerUp.AmountToAdd();

            GunCheck();

            Destroy(other.gameObject);
        }
    }

    void GunCheck()
    {
        foreach (Gun gun in guns)
        {
            if (gun.attackLevelRequirement <= attackLevel)
            {
                gun.isActive = true;
                gun.gameObject.SetActive(true);
            }
        }
    }

    private void RaiseBombAmount(Collider2D other)
    {
        PowerUp powerUp = other.GetComponent<PowerUp>();

        if (powerUp != null && powerUp.isBomb)
        {
            bombAmount = bombAmount + powerUp.AmountToAdd();
        }
        Destroy(other.gameObject);
    }

    public void LowerAttackLevel()
    {
        foreach (Gun gun in guns)
        {
            if (gun.attackLevelRequirement > attackLevel)
            {
                gun.isActive = false;
                gun.gameObject.SetActive(false);
            }
        }
    }

    public int GetAttackLevel()
    {
        return attackLevel;
    }

    public void SetAttackLevel(int value)
    {
        attackLevel = value;
    }
    
    public int GetBombAmount()
    {
        return bombAmount;
    }

    public void SetBombAmount(int value)
    {
        bombAmount = value;
    }
}
