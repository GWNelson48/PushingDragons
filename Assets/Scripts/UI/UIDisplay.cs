using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] Health playerHealth;
    [SerializeField] Shooter shooter;

    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;

    [Header("Bomb")]
    [SerializeField] TextMeshProUGUI bombAmountText;

    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    Scorekeeper scorekeeper;

    GameObject player;

    private void Awake() 
    {
        scorekeeper = FindObjectOfType<Scorekeeper>();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

        playerHealth = player.GetComponent<Health>();
        shooter = player.GetComponent<Shooter>();

        bombAmountText.text = shooter.bombAmount.ToString();
    }

    private void Update() 
    {
        numOfHearts = playerHealth.GetHealth();

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerHealth.GetHealth())
            {
                hearts[i].sprite = fullHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        scoreText.text = scorekeeper.GetScore().ToString("000000000");
        bombAmountText.text = shooter.bombAmount.ToString();
    }
}
