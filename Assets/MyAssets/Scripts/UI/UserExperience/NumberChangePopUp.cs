using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class NumberChangePopUp : MonoBehaviour
{
    private float displayDuration = 2f;
    private Color red = new Color(188f/255f, 0f, 0f, 1f);
    private Color white = new Color(1f, 1f, 1f, 1f);
    private Color green = new Color(0f, 144f/255f, 0f, 1f);
    private Dictionary<TMP_Text, Coroutine> routines = new Dictionary<TMP_Text, Coroutine>();
    private Coroutine currentRoutine;
    private int currentShownNumber;
    public static NumberChangePopUp Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ShowMessage(int number, TMP_Text label)
    {   
        int currentShownNumber = int.Parse(label.text);
        label.text = (currentShownNumber + number).ToString();
        currentShownNumber = int.Parse(label.text);

        if (currentShownNumber > 0)
            label.color = green;
        else if (currentShownNumber < 0)
            label.color = red;
        else
            label.color = white;

        label.gameObject.SetActive(true);

        if (routines.ContainsKey(label) && routines[label] != null)
        {
            StopCoroutine(routines[label]);
        }

        routines[label] = StartCoroutine(HideAfterDelay(label));
    }
    
    private IEnumerator HideAfterDelay(TMP_Text label)
    {
        yield return new WaitForSeconds(displayDuration);

        float fadeTime = 1f;
        float t = 0;

        Color originalColor = label.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            label.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        label.gameObject.SetActive(false);
        label.color = originalColor;
        label.text = "0";

        routines[label] = null; // cleanup
    }
}
