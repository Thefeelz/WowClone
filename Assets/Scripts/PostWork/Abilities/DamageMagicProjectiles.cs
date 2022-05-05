using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMagicProjectiles : DamageMagic
{
    [SerializeField] BaseEnemyStats enemyTarget;
    UPlayerTargetting playerCurrentTarget;

    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (enemyTarget != null)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        transform.LookAt(enemyTarget.transform);
        transform.position = Vector3.MoveTowards(transform.position, enemyTarget.transform.position, Time.deltaTime * abilityMovementSpeed);
        if (Vector3.Distance(transform.position, enemyTarget.transform.position) < 0.5f)
        {
            enemyTarget.TakeDamage(abilityDamage, playerCrit, abilityID, this);
            Destroy(gameObject);
        }
    }

    public void GetTarget(BaseEnemyStats newTarget) 
    { 
        enemyTarget = newTarget;
    }


}
