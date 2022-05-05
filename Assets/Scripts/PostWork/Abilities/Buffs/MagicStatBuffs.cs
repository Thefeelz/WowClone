using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStatBuffs : MagicBuffs
{
    [Header("Magic Buff Attributes")]
    [Header("Stamina")]
    [SerializeField] bool staminaBuff;
    [Range(0,100)][SerializeField] float staminaPercentageBuff;
    [Header("Intellect")]
    [SerializeField] bool intellectBuff;
    [Range(0, 100)] [SerializeField] float intellectPercentageBuff;
    [Header("Spirit")]
    [SerializeField] bool spiritBuff;
    [Range(0, 100)] [SerializeField] float spiritPercentageBuff;
    [Header("Haste")]
    [SerializeField] bool hasteBuff;
    [Range(0, 100)] [SerializeField] float hastePercentageBuff;
    [Header("Crtical Strike")]
    [SerializeField] bool criticalStrikeBuff;
    [Range(0, 100)] [SerializeField] float criticalStrikePercentageBuff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetStaminaPercentage()
    {
        if (staminaBuff)
            return (staminaPercentageBuff / 100) + 1;
        return 1;
    }
    public float GetIntellectPercentage()
    {
        if (intellectBuff)
            return (intellectPercentageBuff / 100) + 1;
        return 1;
    }
    public float GetSpiritPercentage()
    {
        if (spiritBuff)
            return (spiritPercentageBuff / 100) + 1;
        return 1;
    }
    public float GetHastePercentage()
    {
        if (hasteBuff)
            return (hastePercentageBuff / 100) + 1;
        return 1;
    }
    public float GetCriticalStrikePercentage()
    {
        if (criticalStrikeBuff)
            return (criticalStrikePercentageBuff / 100) + 1;
        return 1;
    }
}
