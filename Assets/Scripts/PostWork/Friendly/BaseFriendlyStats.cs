using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseFriendlyStats : MonoBehaviour
{
    [Header("UI Things")]
    [SerializeField] Sprite friendlyPortrait;
    [SerializeField] Canvas friendlyCanvas;
    [SerializeField] Image healthBarImage;
    [SerializeField] GameObject floatingTextPrefab;
    [SerializeField] string friendlyName;
    [SerializeField] int friendlyLevel;
    [Header("Friendly Stats")]
    [SerializeField] int currentFriendlyHealth;
    [SerializeField] int maxFriendlyHealth;
    [SerializeField] int currentFriendlyResource;
    [SerializeField] int maxFriendlyResource;
    [Header("Death Things")]
    [SerializeField] int friendlyID;
    [SerializeField] bool currentlyTargetted = false;

    BasePlayerStats player;

    // Start is called before the first frame update
    void Start()
    {
        currentFriendlyHealth = maxFriendlyHealth;
        currentFriendlyResource = maxFriendlyResource;
        player = FindObjectOfType<BasePlayerStats>();
        friendlyCanvas.GetComponentInChildren<Text>().text = "[" + friendlyLevel + "] " + friendlyName;
    }

    // Update is called once per frame
    void Update()
    {
        friendlyCanvas.transform.LookAt(Camera.main.transform);
    }

    //===============================================
    //  GETTERS AND SETTER FOR FRIENDLY NPC CLASS
    //===============================================

    public float GetCurrentFriendlyHealth() { return currentFriendlyHealth; }
    public void SetCurrentFriendlyHealth(int newValue) { currentFriendlyHealth = newValue; }
    public float GetMaxFriendlyHealth() { return maxFriendlyHealth; }
    public void SetMaxFriendlyHealth(int newValue) { maxFriendlyHealth = newValue; }
    public float GetCurrentFriendlyResource() { return currentFriendlyResource; }
    public void SetCurrentFriendlyResource(int newValue) { currentFriendlyResource = newValue; }
    public float GetMaxFriendlyResource() { return maxFriendlyResource; }
    public void SetMaxFriendlyResource(int newValue) { maxFriendlyResource = newValue; }
    public void SetCurrentlyTargetted(bool newValue)
    {
        if (newValue)
            friendlyCanvas.GetComponentInChildren<Text>().color = Color.green;
        else
            friendlyCanvas.GetComponentInChildren<Text>().color = Color.black;
        currentlyTargetted = newValue;
    }
    public bool GetCurrentlyTargetted() { return currentlyTargetted; }
    public int GetFriendlyLevel() { return friendlyLevel; }
    public string GetFriendlyName() { return friendlyName; }
    public Sprite GetFriendlyPortrait() { return friendlyPortrait; }
    public float GetFriendlyHealthBarFill() { return (float)currentFriendlyHealth / maxFriendlyHealth; }
    public float GetFriendlyResourceBarFill() { return (float)currentFriendlyResource / maxFriendlyResource; }

    public int GetFriendlyID() { return friendlyID; }
}
