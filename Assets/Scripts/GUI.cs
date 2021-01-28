using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
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
        if (Input.GetKeyDown("a"))
        {
            energy += 0.1f;
            if (energy > 1)
            {
                energy = 1;
            }
        }
        else if (Input.GetKeyDown("b"))
        {
            energy -= 0.1f;
            if (energy < 0)
            {
                energy = 0;
            }
        }

        energySlider.value = energy;
    }
}
