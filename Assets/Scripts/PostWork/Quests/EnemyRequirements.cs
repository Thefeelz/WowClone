using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyRequirements
{
    [SerializeField] BaseEnemyStats enemyToKill;
    [SerializeField] int amountToKill;

    public int currentKillCount = 0;
    public bool isSatisfied = false;

    public bool AreRequirementsMet() {return isSatisfied;}
    public int GetCurrentKillCount() { return currentKillCount; }
    public void IncrimentQuestKillCount() 
    {
        if (isSatisfied) { return; }
        currentKillCount++;
        if (currentKillCount >= amountToKill)
            isSatisfied = true;
    }
    public int GetEnemyTypeID () { return enemyToKill.GetEnemyID(); }
    public BaseEnemyStats GetEnemyForEnemyRequirements() { return enemyToKill; }
    public int GetTotalAmountToFulfillRequirements() { return amountToKill; }
}
