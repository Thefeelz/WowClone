using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTManager : MonoBehaviour
{
    public List<DamageMagicDoT> magicDotList = new List<DamageMagicDoT>();
    BaseEnemyStats enemy;
    private void Start()
    {
        enemy = GetComponent<BaseEnemyStats>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CycleThroughDotLists();
    }

    public void CycleThroughDotLists()
    {
        if(magicDotList.Count == 0) { return; }
        for(int i = 0; i < magicDotList.Count; i++)
        {
            magicDotList[i].IncrimentTime();
            if(magicDotList[i].CheckDoDamage())
            {
                enemy.TakeDamage(magicDotList[i].GetAbilityDamage(), magicDotList[i].GetSnapShottedCrit(), magicDotList[i].GetAbilityID(), magicDotList[i]);
            }
            if (magicDotList[i].IsExpired())
                magicDotList.Remove(magicDotList[i]);
        }
    }
    public void AddDotToList(DamageMagicDoT newDot)
    {
        
        for(int i = 0; i < magicDotList.Count; i++)
        {
            if(magicDotList[i].GetAbilityID() == newDot.GetAbilityID())
            {
                Destroy(magicDotList[i].gameObject, .1f);
                magicDotList.RemoveAt(i);
            }
        }
        magicDotList.Add(newDot);
        newDot.transform.SetParent(GetComponent<DebuffManager>().transform);
    }
    public List<DamageMagicDoT> GetList() { return magicDotList; }
    
}
