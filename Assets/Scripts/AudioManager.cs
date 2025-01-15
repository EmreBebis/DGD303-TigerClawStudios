using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Source---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---Audio Clip---")]
    public AudioClip background; // Menü müziði
    public AudioClip playing; // Oyun müziði
    public AudioClip takeDamage; // Hasar sesi
    public AudioClip takePizzas; // Pizza toplama sesi
    public AudioClip playershoot; // Oyuncu ateþ sesi
    public AudioClip enemyExplode; // Enemy patlama sesi
    public AudioClip oyuncuExplode; // Oyuncu patlama sesi

    private void Start()
    {
        PlayMusic(background); // Baþlangýçta menü müziðini çal
    }

    public void PlaySFX(AudioClip clip)
    {
        if (SFXSource != null && clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
        }
    }
}
