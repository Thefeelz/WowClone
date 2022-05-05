using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_HoverInformation : MonoBehaviour
{
    [SerializeField] string title = "";
    [TextAreaAttribute(5, 10)]
    [SerializeField] string description = "";
    private void Start()
    {
        if (GetComponent<BaseEnemyStats>())
            title = GetComponent<BaseEnemyStats>().GetEnemyName() + ", Level " + GetComponent<BaseEnemyStats>().GetEnemyLevel();
        else if (GetComponent<BaseFriendlyStats>())
            title = GetComponent<BaseFriendlyStats>().GetFriendlyName() + ", Level " + GetComponent<BaseFriendlyStats>().GetFriendlyLevel();
    }
    public string GetTitle() { return title; }
    public string GetDescription() { return description; }
}
