using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void Again()
    {
        SceneManager.LoadScene("MatchScene");
        MusicManager.Instance.PlayMusic("Battle");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        MusicManager.Instance.PlayMusic("Menu");
    }
}
