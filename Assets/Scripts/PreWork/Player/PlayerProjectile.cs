//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerProjectile : MonoBehaviour
//{
//    protected GameObject target;
//    protected PlayerTarget playerTarget;
//    [SerializeField] protected float movementSpeed;
//    [SerializeField] protected float damageAmount;
//    [SerializeField] protected float abilityCost;
//    [SerializeField] protected float castTime;
//    [SerializeField] protected float abilityRange;
//    [SerializeField] protected Sprite abilityImage;
//    [SerializeField] protected string abilityName;
//    [SerializeField] protected float cooldownTime;
//    protected PlayerStats playerStats;
//    protected bool criticalStrike = false;
//    EnemyStats targetToHit;
//    public void Awake()
//    {
//        playerStats = FindObjectOfType<PlayerStats>();
//        playerTarget = FindObjectOfType<PlayerTarget>();
//        targetToHit = playerTarget.GetPlayerTarget().GetComponent<EnemyStats>();
//        transform.SetParent(null);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        transform.LookAt(targetToHit.transform);
//        transform.position = Vector3.MoveTowards(transform.position, targetToHit.transform.position, Time.deltaTime * movementSpeed);
//        if(Vector3.Distance(transform.position, targetToHit.transform.position) < 0.5f)
//        {
//            targetToHit.TakeDamage(DetermineDamage(), criticalStrike);
//            Destroy(gameObject);
//        }
//    }
//    public float GetAbilityCost()
//    {
//        return abilityCost;
//    }
//    public float GetAbilityCastTime()
//    {
//        return castTime;
//    }
//    public Sprite GetAbilityImage()
//    {
//        return abilityImage;
//    }
//    public float GetAbilityRange()
//    {
//        return abilityRange;
//    }
//    public string GetAbilityName()
//    {
//        return abilityName;
//    }
//    public float GetAbilityCooldownTime()
//    {
//        return cooldownTime;
//    }
//    protected float DetermineDamage()
//    {
//        if(Mathf.Floor(playerStats.GetPlayerCriticalStrike() + playerStats.GetPlayerIntellect() / 2) > Random.Range(0, 100))
//        {
//            criticalStrike = true;
//            return Mathf.Floor(Random.Range(damageAmount, damageAmount + playerStats.GetPlayerIntellect())) * 2;
//        }
//        return Mathf.Floor(Random.Range(damageAmount, damageAmount + playerStats.GetPlayerIntellect()));
//    }
//    public void SetProjectileTarget(EnemyStats target)
//    {
//        targetToHit = target;
//    }
//}
