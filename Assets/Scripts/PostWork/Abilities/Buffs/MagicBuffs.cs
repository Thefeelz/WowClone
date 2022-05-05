using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBuffs : FriendlyMagic
{
    [SerializeField] int buffMaxDuration;
    [SerializeField] float buffCurrentDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetMaxBuffDuration() { return buffMaxDuration; }
    public float GetBuffCurrentDuration() { return buffCurrentDuration; }
    public void SetBuffCurrentDuration(float _currentDuration) { buffCurrentDuration = _currentDuration; }
    public void DecrementCurrenActiveBuffDuration() { buffCurrentDuration -= Time.deltaTime; }
    public float GetBuffFillAmount() { return buffCurrentDuration / buffMaxDuration; }
}
