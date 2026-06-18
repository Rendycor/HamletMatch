using UnityEngine;
using UnityEngine.UI;


public class CastleScript : MonoBehaviour
{
    [SerializeField] protected string opposingUnitLayerName;
    [SerializeField] protected GameObject canvasToShowWhenDestroyed;
    [SerializeField] protected string songNameWhenDestroyed;
    [SerializeField] protected Image healthBar;


    private float health = 3;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test");
        if (collision.gameObject.layer == LayerMask.NameToLayer(opposingUnitLayerName))
        {
            takeDamageAndCheck();
            SoundManager.Instance.PlaySound2D("Door");
            Destroy(collision.gameObject);
            
        }
    }
    private void takeDamageAndCheck()
    {
        health--;
        healthBar.fillAmount = health / 3;
        if (health == 0)
        {
            MusicManager.Instance.PlayMusic(songNameWhenDestroyed);
            canvasToShowWhenDestroyed.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
