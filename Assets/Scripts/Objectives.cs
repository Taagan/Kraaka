using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    public GameObject topToggle;
    public GameObject midToggle;
    public GameObject botToggle;

    private List<GameObject> toggles;
    private int toggleIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        toggles = new List<GameObject>();
        toggles.Add(topToggle);
        toggles.Add(midToggle);
        toggles.Add(botToggle);
        foreach (GameObject t in toggles)
        {
            t.GetComponent<Toggle>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleIndex >= toggles.Count)
        {
            toggleIndex = 0;
            foreach (GameObject t in toggles)
            {
                MakeToggleInvis(t);
            }
        }
        if (Input.GetKeyDown("space"))
        {
            ChangeToggleText(toggles[toggleIndex], "Hi my name is boo");
            toggleIndex++;
        }
    }


    void ChangeToggleText(GameObject toggle, string text)
    {
        toggle.GetComponentInChildren<Text>().text = text;
        MakeToggleVisible(toggle);
    }

    void MakeToggleInvis(GameObject target)
    {
        target.active = false;
    }

    void MakeToggleVisible(GameObject target)
    {
        target.active = true;
    }

    void ObjectiveComplete(GameObject objective)
    {
        MakeToggleVisible(objective);
    }
}
