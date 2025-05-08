using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource _musicSource;  
    [SerializeField] private AudioSource _sfxSource;    

    [SerializeField] private AudioClip _musicClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _collectClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (_musicSource == null) _musicSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }
    public void PlayMusic(float volume = 0.5f)
    {
        if (_musicClip == null || _musicSource == null) return;
        _musicSource.clip = _musicClip;
        _musicSource.loop = true;
        _musicSource.volume = volume;    
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource?.Stop();
    }
    public void PlayJump(float volume = 0.5f)
    {
        if (_jumpClip == null || _sfxSource == null) return;
        _sfxSource.volume = volume;
        _sfxSource.PlayOneShot(_jumpClip);
    }
    public void PlayCollect(float volume = 0.5f)
    {
        if (_collectClip == null || _sfxSource == null) return;
        _sfxSource.volume = volume;
        _sfxSource.PlayOneShot(_collectClip);
    }
}
