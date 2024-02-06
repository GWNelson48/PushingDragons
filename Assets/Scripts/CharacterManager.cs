using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public Character[] characters;
    public Character currentCharacter;

    LevelManager levelManager;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        levelManager = FindObjectOfType<LevelManager>();
    }

    private void Start()
    {
        if (characters.Length > 0)
        {
            currentCharacter = characters[0];
        }
    }

    public void ChooseKazarian()
    {
        // This chooses the first element in the characters array AKA Karzarian
        currentCharacter = characters[0];
        levelManager.NoLoadNextLevel();
    }

    public void ChooseElmae()
    {
        currentCharacter = characters[1];
        levelManager.NoLoadNextLevel();
    }

    public void ChooseJerry()
    {
        currentCharacter = characters[2];
        levelManager.NoLoadNextLevel();
    }
}
