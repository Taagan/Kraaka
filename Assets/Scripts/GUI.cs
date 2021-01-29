using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public GameObject kraaka;
    public Slider energySlider;
    public int moralMeter;
    public float energy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        energy = kraaka.GetComponent<Kraakscript2>().currentEnergy / kraaka.GetComponent<Kraakscript2>().maxEnergy;

            if (energy > 1)
            {
                energy = 1;
            }

            if (energy < 0)
            {
                energy = 0;
            }

        energySlider.value = energy;
    }
}
