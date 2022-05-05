using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMagicDoT : DamageMagic
{
    [SerializeField] float tickRate;
    [SerializeField] float durationOnTarget;

    float elapsedTimeDoT = 0f;
    int numberOfTicks = 1;
    private void Start()
    {
        
    }
    public float GetTickRate() { return tickRate; }
    public void SetTickRate(float _tickRate) { tickRate = _tickRate; }
    public float GetDurationOnTarget() { return durationOnTarget; }
    public float IncrimentTime() { return elapsedTimeDoT += Time.deltaTime; }

    public bool CheckDoDamage()
    {
        if(elapsedTimeDoT > tickRate * numberOfTicks)
        {
            numberOfTicks++;
            return true;
        }
        return false;
    }
    public bool IsExpired()
    {
        if(durationOnTarget < elapsedTimeDoT) 
        {
            Destroy(gameObject, 0.1f);
            return true; 
        }
        return false;
    }
    public float GetDoTFillProgress() { return 1 - elapsedTimeDoT / durationOnTarget; }
}
