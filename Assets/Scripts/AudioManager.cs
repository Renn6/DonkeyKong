using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Sound effects")]
    public AudioClip jumpSound;
    public AudioClip rescueSound;
    public AudioClip extraLifeSound;
    public AudioClip deathSound;
    public AudioClip buttonSound;

    [Range(0f, 1f)] public float musicVolume = 0.4f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    public void PlayJump() => PlaySfx(jumpSound);
    public void PlayRescue() => PlaySfx(rescueSound);
    public void PlayExtraLife() => PlaySfx(extraLifeSound);
    public void PlayDeath() => PlaySfx(deathSound);
    public void PlayButton() => PlaySfx(buttonSound);
}