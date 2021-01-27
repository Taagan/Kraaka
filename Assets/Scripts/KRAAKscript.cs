using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class KRAAKscript : MonoBehaviour
{
    
    public float flightSpeed = 20f; //speed added when flapping
    public float maxSpeed = 60f;//max speed reacheable by flapping, dive to reach higher
    public float minSpeed = 20f;
    public float friction = .05f;//percent of speed lost each second, looses none below minSpeed

    protected float flapTimer = 0;
    public float flapCooldown = .3f;
    public float flapEnergyCost = 20f;

    public float maxEnergy = 100f;
    public float energyRechargeRate = 10f;//energy per second
    public float currentEnergy = 0;

    public float rotationSpeed = 90f;
    private sbyte rotationDirection = 0; //-1 = vänster, 0 = nada, 1 = höger
    
    protected Vector3 velocity;//vec3 to simplify update, z dimension not used

    protected bool facingRight = true;//used to determine which direction on the bird is up
    public float facingSwapMargin = Mathf.PI / 4;//angle you need to reach to flip, allows you to fly slightly upside down for a while, optionally allow player to swap with a button
    public Vector3 forwardVector { get; }
    public Vector3 upVector {
        get
        {
            if (facingRight)
                return -transform.right;
            else
                return transform.right;
        }
    }//fixa, ger antingen transform.Left eller transform.Right beroende på rotation, så att

    //implementera lite senare, om tittar neråt inom den vinkeln av rakt ner, genereras ingen lyft, endast 
    protected bool diving = false;
    public float diveZoneAngle = Mathf.PI / 4;

    public float gravity = 9.82f;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(40, 0);
        Debug.Log("START");
    }

    public void Flap()
    {
        if(flapTimer <= 0 && currentEnergy > flapEnergyCost)
        {
            float totalSpeed = velocity.magnitude;
            if (totalSpeed < maxSpeed)
                velocity += transform.up * Mathf.Min((flightSpeed - ((flightSpeed + totalSpeed) - maxSpeed)), flightSpeed); //jävla härva, tror blir rätt dock....

            flapTimer = flapCooldown;
            currentEnergy -= flapEnergyCost;

            Debug.Log("FLAP");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"> -1 = LEFT, 0 = Nothing, 1 = RIGHT </param>
    public void Rotate(sbyte direction)
    {
        rotationDirection = direction;
    }
    
    // Update is called once per frame
    void Update()
    {
        float dT = Time.deltaTime;

        //KEYBOARDCONTROLLS, Flytta nån annanstans sen, typ till controller klass så man kan göra ai till
        #region Keyboardcontrolls

        if (Input.GetKey(KeyCode.LeftArrow))
            Rotate(-1);
        else if (Input.GetKey(KeyCode.RightArrow))
            Rotate(1);

        if (Input.GetKey(KeyCode.Space))
            Flap();

        #endregion

        if(flapTimer > 0)
            flapTimer -= dT;
        

        #region fysik
        //lift = gravity fast riktad 
        velocity.y -= gravity * dT;
        velocity += upVector * gravity * dT;

        transform.Rotate(0, 0, rotationDirection * -rotationSpeed * dT);

        if (facingRight)
        {
            //kolla om vinkel överstiger gräns, isåfall byt facingRight
            if (transform.rotation.eulerAngles.z > facingSwapMargin)
                facingRight = false;
            //play flip animation
        }
        else if (!facingRight)
        {
            if (transform.rotation.eulerAngles.z < -facingSwapMargin)
                facingRight = true;
            //play flipp-animation
        }

        #endregion

        Debug.DrawLine(transform.position, transform.position + upVector * 20, Color.red);

        transform.position += velocity * dT;
        rotationDirection = 0;
    }
}
