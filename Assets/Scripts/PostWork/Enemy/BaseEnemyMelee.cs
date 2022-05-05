using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyMelee : BaseEnemyStats
{
    [SerializeField] int meleeRange;
    [SerializeField] int meleeDamage;
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(enemyState == EnemyState.Idle)
        {
            ProcessIdle();
        }
        else if (enemyState == EnemyState.Chasing)
        {
            ProcessChasing();
        }
        else if (enemyState == EnemyState.Attacking)
        {
            ProcessAttacking();
        }
        else if (enemyState == EnemyState.Fleeing)
        {
            // Fleeing Stuff Here
        }
        else if (enemyState == EnemyState.ReturnHome)
        {
            // Return Home Stuff Here
        }
        else if (enemyState == EnemyState.Dead)
        {
            // Dead Stuff here
        }
    }
    public int GetMeleeDamage() { return meleeDamage; }
    void ProcessIdle()
    {
        if (!player) { return; }
        if(Helper.GetInLineOfSightAndRange(player.transform, transform, maxDetectionRange))
        {
            enemyState = EnemyState.Chasing;
            enemyAnim.SetBool("chasing", true);
        }
    }
    void ProcessChasing()
    {
        transform.LookAt(player.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, runSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, player.transform.position) <= meleeRange)
        {
            enemyState = EnemyState.Attacking;
            enemyAnim.SetBool("chasing", false);
            enemyAnim.SetBool("attack", true);
        }
    }
    void ProcessAttacking()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > meleeRange)
        {
            enemyAnim.SetBool("attack", false);
            enemyAnim.SetBool("chasing", true);
            enemyState = EnemyState.Chasing;
        }
    }
}
