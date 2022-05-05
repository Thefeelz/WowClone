using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    Text errorText;
    void Start()
    {
        errorText = GetComponent<Text>();
        errorText.gameObject.SetActive(false);
    }
    public void SetMessageToScreen(string message, float timeForDisplay)
    {
        StopAllCoroutines();
        errorText.gameObject.SetActive(true);
        errorText.text = message;
        StartCoroutine(DisplayMessage(timeForDisplay));
    }
    IEnumerator DisplayMessage(float timeForDisplay)
    {
        
        yield return new WaitForSeconds(timeForDisplay);
        errorText.gameObject.SetActive(false);
    }
}
