using UnityEngine;

public class EndScreenTransition : MonoBehaviour
{

    public void Defeated(){
        Time.timeScale = 0f;
    }

    public void Victorious(){
        Time.timeScale = 0f;
    }
}
