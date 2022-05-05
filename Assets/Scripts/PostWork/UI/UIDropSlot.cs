using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped");
        if(eventData.pointerDrag != null)
        {
            GetComponent<SpellID>().SetSpellID(eventData.pointerDrag.GetComponent<SpellID>().GetSpellID());
            Sprite image = FindObjectOfType<BasePlayerStats>().GetPlayerSpellbook().GetAbilityFromList(GetComponent<SpellID>().GetSpellID()).GetAbilityImage();
            foreach (Transform _transform in transform)
            {
                if (_transform.CompareTag("UIHotkeySaturatedBackground"))
                    _transform.GetComponent<Image>().sprite = image;
                else if (_transform.CompareTag("UIHotkeyButton"))
                    _transform.GetComponent<Image>().sprite = image;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
