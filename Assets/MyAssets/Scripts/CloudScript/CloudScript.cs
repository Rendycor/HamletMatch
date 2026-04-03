using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour
{
    [Header("Needed")]
    [SerializeField] private GameObject cloudprefab;
    private GameObject[] clouds = new GameObject[4];

    void Awake()
    {
        for(int x = 0; x < clouds.Length; x++)
        {
            clouds[x] = Instantiate(cloudprefab, new Vector3(Random.Range(-10,10),transform.position.y,0), Quaternion.identity, transform);
        }
    }

    void Update()
    {
        for(int x = 0; x < clouds.Length; x++)
        {
            clouds[x].transform.position += new Vector3(-0.05f * (x+2) * Time.deltaTime, 0, 0);
            if (clouds[x].transform.position.x < -11)
            {
                clouds[x].transform.position += new Vector3(21f, 0, 0);
            }
        }
        
    }
}
