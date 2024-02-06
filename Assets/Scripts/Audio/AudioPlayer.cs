using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer instance;

    [Header("Shooting")]
    [SerializeField] AudioClip shootingClip;
    [SerializeField] [Range(0f, 1f)] float shootingVolume = 1f;

    [Header("Damage")]
    [SerializeField] AudioClip damageClip;
    [SerializeField] [Range(0f, 1f)] float damageVolume = 1f;

    [Header("Death")]
    [SerializeField] AudioClip deathClip;
    [SerializeField] [Range(0f, 1f)] float deathVolume = 1f;

    [Header("BGM")]
    [SerializeField] public float musicVolume = 0.5f;
    [SerializeField] public float transitionTime = 2f;
    public AudioClip[] MusicClips;
    
    public AudioSource audioSource;

    void Awake() 
    {
        ManageSingleton();
        audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void ManageSingleton()
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

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (this.isActiveAndEnabled)
        {
            switch (currentScene)
            {
                // Main Menu
                case 0:
                    audioSource.Stop();
                    break;
                // Character Select
                case 2:
                    PlayStageBGM(MusicClips[4]);
                    break;
                // Stage 1
                case 4:
                    PlayStageBGM(MusicClips[0]);
                    break;
                // Stage 2
                case 6:
                    PlayStageBGM(MusicClips[1]);
                    break;
                // Stage 3
                case 8:
                    PlayStageBGM(MusicClips[2]);
                    break;
                // Stage 4 Final Boss
                case 10:
                    PlayStageBGM(MusicClips[3]);
                    break;
                // Win Screen
                case 11:
                    PlayStageBGM(MusicClips[5]);
                    break;
                default:
                    audioSource.Stop();
                    break;
            }
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        musicVolume = 0.01f;
        StartCoroutine(FadeAudioSource.StartFade(audioSource, transitionTime, musicVolume));
    }

    void PlayStageBGM(AudioClip music)
    {
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
        StartCoroutine(FadeAudioSource.StartFade(audioSource, transitionTime, musicVolume));
    }

    public void PlayShootingClip()
    {
        PlayClip(shootingClip, shootingVolume);
    }

    public void PlayDamageClip()
    {
        PlayClip(damageClip, damageVolume);
    }

    public void PlayDeathClip()
    {
        PlayClip(deathClip, deathVolume);
    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (clip != null)
        {
            Vector2 cameraPos = Camera.main.transform.position;
            AudioSource.PlayClipAtPoint(clip, cameraPos, volume);
        }
    }
}
