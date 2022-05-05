using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int goldAmount;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetGoldAmount()
    {
        return goldAmount;
    }
    public void ChangeGoldAmount(int gold)
    {
        goldAmount += gold;
    }
}
