using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour
{
    [Header("Needed")]
    [SerializeField] private GameObject cloudprefab;
    private GameObject[] clouds = new GameObject[8];
    private RectTransform[] cloudsTransform = new RectTransform[8];
    private float cloudAreaWidth ;


    void Awake()
    {
        RectTransform thisRectransform = GetComponent<RectTransform>();
        cloudAreaWidth = thisRectransform.sizeDelta.x;
        for(int x = 0; x < clouds.Length; x++)
        {
            clouds[x] = Instantiate(cloudprefab, transform.position, Quaternion.identity, transform);
            cloudsTransform[x] = clouds[x].GetComponent<RectTransform>();
            cloudsTransform[x].anchoredPosition = new Vector2(Random.Range(-cloudAreaWidth/2, cloudAreaWidth/2),cloudsTransform[x].anchoredPosition.y);
        }
    }

    void Update()
    {
        Vector2 currentPositon;
        Vector2 targetPositon;
        for(int x = 0; x < clouds.Length; x++)
        {
            currentPositon = cloudsTransform[x].anchoredPosition;
            targetPositon =  new Vector2(currentPositon.x - Random.Range(10f,44f),currentPositon.y);
            cloudsTransform[x].anchoredPosition = Vector2.Lerp(currentPositon, targetPositon, Time.deltaTime *1f);
            if (currentPositon.x < -cloudAreaWidth/2 - cloudsTransform[x].sizeDelta.x)
            {
                cloudsTransform[x].anchoredPosition = new Vector2(cloudAreaWidth/2+cloudsTransform[x].sizeDelta.x,cloudsTransform[x].anchoredPosition.y);
            }
        }
        
    }
}
