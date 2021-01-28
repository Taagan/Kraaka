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

    public string topObjective;
    public string midObjective;
    public string botObjective;

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
            t.GetComponent<Toggle>().isOn = false;
        }

        ChangeToggleText(topToggle, topObjective);
        ChangeToggleText(midToggle, midObjective);
        ChangeToggleText(botToggle, botObjective);

    }

    // Update is called once per frame
    void Update()
    {
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

    void ObjectiveComplete(Toggle objective)
    {
        objective.isOn = true;
    }
}
