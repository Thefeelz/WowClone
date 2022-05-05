using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGlowToggle : MonoBehaviour
{
    [SerializeField] List<GameObject> glowyHands = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleHandGlow()
    {
        foreach (GameObject gameObject in glowyHands)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
