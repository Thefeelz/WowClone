using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantPunch : MonoBehaviour
{
    BaseEnemyMelee stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponentInParent<BaseEnemyMelee>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Attack()
    {
        FindObjectOfType<BasePlayerStats>().TakeDamage(stats.GetMeleeDamage());
    }
}
