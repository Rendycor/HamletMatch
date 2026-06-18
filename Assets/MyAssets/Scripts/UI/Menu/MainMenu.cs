using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

 
public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;
 
    private void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("Menu");
    }
 
    public void Play()
    {
        SceneManager.LoadScene("MatchScene");
        MusicManager.Instance.PlayMusic("Battle");

    }
 
    public void Quit()
    {
        Application.Quit();
    }
 
    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
 
    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }
 
    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
 
        audioMixer.GetFloat("SoundVolume", out float soundVolume);
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
    }
 
    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SoundVolume");
    }
}