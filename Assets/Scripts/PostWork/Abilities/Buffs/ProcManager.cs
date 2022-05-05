using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcManager : MonoBehaviour
{
    [SerializeField] List<Procs> procManager = new List<Procs>();
    BasePlayerStats player;
    SpellFlash spellFlash;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<BasePlayerStats>();
        spellFlash = FindObjectOfType<SpellFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        if (procManager.Count > 0)
            UpdateProcList();
    }
    public void AddProcToList(Procs newProc)
    {
        for(int i = 0; i < procManager.Count; i++)
        {
            if(procManager[i].GetBuffID() == newProc.GetBuffID())
            {
                procManager[i].AddAStackToTheBuff();
                HandleProcEffects(procManager[i]);
                return;
            }
        }
        procManager.Add(newProc);
        newProc.AddAStackToTheBuff();
        HandleProcEffects(newProc);
    }
    void UpdateProcList()
    {
        for(int i = 0; i < procManager.Count; i++)
        {
            procManager[i].DecrementBuffDuration();
            if(procManager[i].GetBuffCurrentDuration() <= 0)
            {
                RemoveProcFromList(procManager[i]);
            }
        }
    }
    public void RemoveProcFromList(Procs proc)
    {
        Debug.Log("Removed " + proc.name + " from the List");
        procManager.Remove(proc);
        proc.ResetBuff(proc.GetAbilityIdToEffect(), player);
    }
    void HandleProcEffects(Procs newProc)
    {
        spellFlash.SetAbilityToSpellFlash(newProc);
        if (player.GetClassType() == BasePlayerStats.ClassType.Intellect)
            player.GetComponent<MagicUser>().HandleMagicProcs(newProc);
    }
    public void RemoveProcFromListOnSpellUse(int abilityID)
    {
        for(int i = 0; i < procManager.Count; i++)
        {
            if(procManager[i].GetAbilityIdToEffect() == abilityID)
            {
                procManager[i].ResetBuff(abilityID, player);
                procManager.RemoveAt(i);
            }
        }
    }
}
