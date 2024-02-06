using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Scorekeeper scorekeeper;
    AudioPlayer audioPlayer;
    PlayerStats playerStats;

    public Animator transition;

    public float transitionTime = 2f;
    public int currentLevel;

    int gameOverScene;

    void Start()
    {
        scorekeeper = FindObjectOfType<Scorekeeper>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        playerStats = FindObjectOfType<PlayerStats>();

        gameOverScene = SceneManager.sceneCountInBuildSettings - 1;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartGame()
    {
        scorekeeper.ResetScore();
        NoLoadNextLevel();
    }

    public void NextLevel()
    {
        currentLevel++;
        playerStats.SetRetryLevel(currentLevel);
        StartCoroutine(LoadLevel(currentLevel));
    }

    public void NoLoadNextLevel()
    {
        currentLevel++;
        playerStats.SetRetryLevel(currentLevel);
        SceneManager.LoadScene(currentLevel);
    }

    public void MainMenu()
    {
        currentLevel = 0;
        playerStats.SetRetryLevel(0);
        scorekeeper.ResetScore();
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        scorekeeper.ResetScore();
        playerStats.attackAmount = 3;
        playerStats.bombAmount = 5;
        StartCoroutine(LoadLevel(playerStats.GetRetryLevel()));
    }

    public void GameOver()
    {
        StartCoroutine(LoadLevel(gameOverScene));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        StartCoroutine(FadeAudioSource.StartFade(audioPlayer.audioSource, transitionTime, 0.01f));
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
        transition.SetTrigger("Start");
        StartCoroutine(FadeAudioSource.StartFade(audioPlayer.audioSource, transitionTime, audioPlayer.musicVolume));
    }
}
