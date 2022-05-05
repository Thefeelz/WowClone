using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInCombatTracker : MonoBehaviour
{
    [SerializeField] List<BaseEnemyStats> enemiesInCombat = new List<BaseEnemyStats>();
    bool inCombat = false;
    CombatLog combatLog;
    // Start is called before the first frame update
    void Start()
    {
        combatLog = GetComponent<CombatLog>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddEnemyToList(BaseEnemyStats enemyToAdd)
    {
        inCombat = true;
        enemiesInCombat.Add(enemyToAdd);
        if (!combatLog.GetCurrentlyLogging())
            combatLog.StartCombat();
    }
    public void RemoveEnemyFromList(BaseEnemyStats enemyToRemove)
    {
        enemiesInCombat.Remove(enemyToRemove);
        if (enemiesInCombat.Count == 0)
        {
            inCombat = false;
            GetComponent<CombatLog>().CombatEnded();
        }
    }
    public bool GetInCombat() { return inCombat; }
}
