//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class DoT : PlayerProjectile
//{
//    [SerializeField] float abilityTickSpeed;
//    [SerializeField] float abilityDuration;

//    int tickMoment = 1;
//    public float timeLeft = 0f;
//    public EnemyStats enemy;
//    bool completed = false;
//    // Start is called before the first frame update
//    new void Start()
//    {
//        base.Awake();
//        timeLeft = abilityDuration;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        timeLeft -= Time.deltaTime;
//        if(timeLeft < 0) { enemy.RemoveDoTFromList(this);}
//        if (enemy.GetCurrentEnemyHealth() > 0) { DealDamage(); }
//        else { enemy.RemoveDoTFromList(this); }
//    }
//    void DealDamage()
//    {
//        if(timeLeft < (abilityDuration - abilityTickSpeed * tickMoment))
//        {
//            enemy.TakeDamage(DetermineDamage(), criticalStrike);
//            tickMoment++;
//            criticalStrike = false;
//        }
//    }
//    public float GetAbilityTickSpeed() { return abilityTickSpeed; }
//    public void SetAbilityTickSpeed(float value) { abilityTickSpeed = value; }
//    public float GetAbilityDuration() { return abilityDuration; }
//    public void SetAbilityDuration(float value) { abilityDuration = value; }
//    public bool GetDoTComplete() { return completed; }
//    public void SetEnemyForDoT(EnemyStats enemy)
//    {
//        this.enemy = enemy;
//    }
//}

