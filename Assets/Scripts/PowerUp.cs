using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] int amount = 1;
    public bool isHealth;
    public bool isAttack;
    public bool isBomb;
    public float moveSpeed = 1f;

    void Update() 
    {
        Vector2 moveDir = (gameObject.transform.localRotation * Vector2.down).normalized;
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = moveDir * moveSpeed;
        }

        Destroy(gameObject, 12f);
    }

    public int AmountToAdd()
    {
        return amount;
    }
}
