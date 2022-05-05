using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellFlash : MonoBehaviour
{
    Image spellFlash;
    bool spellCurrentlyOnDisplay;
    int currentAbilityIDOnDisplay;
    // Start is called before the first frame update
    void Start()
    {
        spellFlash = GetComponentInChildren<Image>();
        spellFlash.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetAbilityToSpellFlash(Procs proc)
    {
        if (spellCurrentlyOnDisplay)
        {
            StopAllCoroutines();
        }
        spellCurrentlyOnDisplay = true;
        currentAbilityIDOnDisplay = proc.GetAbilityIdToEffect();
        spellFlash.gameObject.SetActive(true);
        spellFlash.sprite = proc.GetProcImage();
        StartCoroutine(TurnOffImage());
    }
    public void CheckNewCastToUseSpellFlash(Abilities ability)
    {
        if(ability.GetAbilityID() == currentAbilityIDOnDisplay)
        {
            spellFlash.gameObject.SetActive(false);
        }
    }

    IEnumerator TurnOffImage()
    {
        yield return new WaitForSeconds(5f);
        spellFlash.gameObject.SetActive(false);
        spellCurrentlyOnDisplay = false;
    }
}
