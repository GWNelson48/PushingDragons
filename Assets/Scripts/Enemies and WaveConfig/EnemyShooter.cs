using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Projectiles")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 3f;
    [SerializeField] float projectileLifetime = 12f;
    public bool isShooter;
    public bool isHoming;

    [Tooltip("This only affects the Lizard Mage")]
    public bool hasShootAnimation;
    Animator myAnimator;

    [Header("Fire Rate")]
    float firingRate;
    [SerializeField] public float minFireRate = 2f;
    [SerializeField] public float maxFireRate = 3f;

    private float timer;

    void Start() 
    {
        myAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isShooter)
        {
            timer += Time.deltaTime;

            if (timer > firingRate)
            {
                timer = 0;
                firingRate = Random.Range(minFireRate, maxFireRate);
                Shoot();
            }
        }
    }

    void Shoot()
    {
        GameObject shot = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();

        if (hasShootAnimation)
        {
            myAnimator.SetTrigger("Attacking");
        }

        if (isHoming)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Vector3 targetPos = player.transform.position;
                Vector2 shotDirection = (targetPos - transform.position).normalized;

                if (rb != null)
                {
                    rb.velocity = shotDirection * projectileSpeed;
                }
            }
        }
        else if (!isHoming && rb != null)
        {
            Vector2 shotDirection = Vector2.down.normalized;
            rb.velocity = shotDirection * projectileSpeed;
        }

        Destroy(shot, projectileLifetime);
    }
}
