using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShooter : MonoBehaviour
{
    [Header("Projectile 1")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 3f;
    [SerializeField] float firingRate = 2f;

    [Header("Projectile 2")]
    [SerializeField] GameObject projectilePrefab2;
    [SerializeField] float projectileSpeed2 = 5f;

    [Header("Projectile Timing")]
    [SerializeField] float timeBetweenVolleys = 2f;
    [SerializeField] float projectileLifetime = 12f;

    [Tooltip("The 2nd projectile type is the only one that has homing capability")]
    public bool isHoming;

    private float timer;
    Animator myAnimator;

    // NOTE: You can always add more arrays to this script.
    // These DO NOT need to be Gun[] because there will be no Gun script on them!
    public GameObject[] gunArray1;
    public GameObject[] gunArray2;
    public GameObject[] gunArray3;

    void Awake() 
    {
        myAnimator = GetComponentInChildren(typeof(Animator)) as Animator;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > firingRate + timeBetweenVolleys)
        {
            timer = 0;

            // Create an event in the Attacking animation to call Shoot();
            myAnimator.SetTrigger("Attacking");
        }
    }

    void Shoot()
    {
        Invoke("Shoot2", timeBetweenVolleys);
        Invoke("Shoot3", timeBetweenVolleys);

        foreach(GameObject gun in gunArray1)
        {
            GameObject shot = Instantiate(projectilePrefab, gun.transform.position, Quaternion.identity);
            Vector2 shotDirection = (gun.transform.localRotation * Vector2.down).normalized;
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = shotDirection * projectileSpeed;
            }

            Destroy(shot, projectileLifetime);
        }
    }

    void Shoot2()
    {
        foreach (GameObject gun in gunArray2)
        {
            GameObject shot = Instantiate(projectilePrefab2, gun.transform.position, Quaternion.identity);
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();

            if (isHoming)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    Vector3 targetPos = player.transform.position;
                    Vector2 shotDirection = (targetPos - transform.position).normalized;

                    if (rb != null)
                    {
                        rb.velocity = shotDirection * projectileSpeed2;
                    }
                }
            }
            else if (!isHoming && rb != null)
            {
                Vector2 shotDirection = (gun.transform.localRotation * Vector2.down).normalized;
                rb.velocity = shotDirection * projectileSpeed2;
            }

            Destroy(shot, projectileLifetime);
        }
    }

    void Shoot3()
    {
        foreach (GameObject gun in gunArray3)
        {
            GameObject shot = Instantiate(projectilePrefab2, gun.transform.position, Quaternion.identity);
            Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();

            if (isHoming)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    Vector3 targetPos = player.transform.position;
                    Vector2 shotDirection = (targetPos - transform.position).normalized;

                    if (rb != null)
                    {
                        rb.velocity = shotDirection * projectileSpeed2;
                    }
                }
            }
            else if (!isHoming && rb != null)
            {
                Vector2 shotDirection = (gun.transform.localRotation * Vector2.down).normalized;
                rb.velocity = shotDirection * projectileSpeed2;
            }

            Destroy(shot, projectileLifetime);
        }
    }

}

