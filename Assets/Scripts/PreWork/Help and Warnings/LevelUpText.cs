using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpText : MonoBehaviour
{
    float timeAwake = 2f;
    float time = 0;
    bool startCountdown = false;

    // Update is called once per frame
    void Update()
    {
        if (startCountdown)
        {
            time += Time.deltaTime;
            if (time > timeAwake) 
            {
                time = 0;
                startCountdown = false;
                gameObject.SetActive(false); 
            }
        }
    }

    public void StartLevelUpText()
    {
        startCountdown = true;
    }
}
