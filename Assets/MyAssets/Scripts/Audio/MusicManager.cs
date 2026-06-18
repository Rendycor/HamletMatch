using System.Collections;
using UnityEngine;
 
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
 
    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;
 
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
 
    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }
 
    IEnumerator Fade(float from, float to, float duration)
    {
        float startTime = Time.unscaledTime;
    
        while (Time.unscaledTime < startTime + duration)
        {
            float t = (Time.unscaledTime - startTime) / duration;
            musicSource.volume = Mathf.Lerp(from, to, t);
            yield return null;
        }
    
        musicSource.volume = to;
    }
    
    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        yield return Fade(1f, 0f, fadeDuration);
    
        musicSource.clip = nextTrack;
        musicSource.Play();
    
        yield return Fade(0f, 1f, fadeDuration);
    }
}