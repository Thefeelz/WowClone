using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] float globalCoolDown = 1f;
    [SerializeField] public string playerName = "";

    public static GameManager Instance;

    public InputField myInput;

    List<BasePlayerStats> allActivePlayersInLevel = new List<BasePlayerStats>();
    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //allActivePlayersInLevel = FindObjectsOfType<BasePlayerStats>();
    }
    public string GetPlayerNameFromMenu() { return playerName; }
    public void SetPlayerNameInMenu(string newPlayerName) { playerName = newPlayerName; }

}
