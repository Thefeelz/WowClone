using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellID : MonoBehaviour
{
    [SerializeField] int spellID = 0;

    public void SetSpellID(int id) { spellID = id; }
    public int GetSpellID() {return spellID; }
}
