using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kraakscript2 : MonoBehaviour
{
    public GameObject cameraGO;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    public float healthPoints = 100;
    public float maxHealthPoints = 100;

    public float InitialVelocity = 40f;

    public float wingDrag = .05f;//percent of speed that disapears each second, lower when diving, increases exponentially with speed, so speed is limited
    public float diveDrag = .01f;
    
    public float flappingAcceleration = 20f;//units per second per second increased while flapping
    public float flappingEnergyCost = 100f;//energy per second consumed while flapping
    public float flappingCooldown = 1f; //cooldown in seconds starts after stopFlapping

    public float maxEnergy = 100f;
    public float energyRegeneration = 100f;//energy recovered per second
    [HideInInspector]
    public float currentEnergy = 100f;

    public float diveBoostSpeed = 100f;//speedbost gotten when exiting dive
    public float diveBoostCooldown = 2f;//seconds between dive boost being accessible
    
    public Vector3 forwardVector { get { return transform.right; } }//borde alltid vara transform.right, om jag tänkt rätt här...
    public Vector3 upVector { get { return facingRight ? transform.up : -transform.up; } }//depending on which way you are flipped it should either be transform.up or -transform.up

    public float gravity = 5f;//amount accelerated or deccelerated when flying upwards or downwards
    
    public float rotationSpeed = 120f;//degrees per second

    public float fwdMomentum = 2f;//used to determin how quickly you change direction after turning
    public float turnMomentum = 1f;//bad names i know...

    public float minSpeedAutoTurningThreshold = 20f;//speed which when the bird is under it starts to automatically turn downwards.
    public float autoTurnMaxStrength = 540; //degrees per second it rotates automatically downwards when slow.
    public float autoTurningMinAngleThreshold = 5f;//angle from bottom which it stops autoturning.

    protected bool facingRight = true;//facingRight = true betyder att fågeln inte är upp och ner när den tittar högerut asså, använd för att hitta upåtvektor
    protected Vector3 velocity;

    protected float diveBoostTimer = 0f;
    protected float flapTimer = 0f;

    protected float currentDrag;

    protected bool diving = false;
    protected bool flapping = false;

    protected sbyte rotationDirection = 0;//-1 = höger, 0 = stilla, +1 = vänster

    protected float rotationCoefficient = 1;//implement later, possible changes in speed of rotation depending on speed and if diving etc..
    
    public bool DEBUG_ARROWS = false;

    /*
     * KORT BESKRIVNING HÄR ASSÅ:
     * fågeln rör sig framåt med fart när vingarna är ute. Flappa me dem för att accelerera, eller flyg neråt.
     * när man "dyker" tar man in vingarna, har mindre drag, och accelererar neråt me gravitationen och kan rotera fritt utan att styra fågeln.
     * När man slutar dyka, om man kan, så aktiveras dykboosten i fågelns framåtriktning och man börjar flyga igen.
     * man kan inte flyga långsammare än minSpeed, då påverkar drag inte en.
     */


    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(InitialVelocity, 0);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void StartFlapping()
    {
        if(flapTimer <= 0)
        {
            flapping = true;
            animator.SetBool("flaxar", true);
            if (diving)
                StopDive();
        }
    }

    public void StopFlapping()
    {
        if (flapping)
        {
            flapping = false;
            animator.SetBool("flaxar", false);
            flapTimer = flappingCooldown;
        }
    }

    public void StartDive()
    {
        diving = true;
        animator.SetBool("dyker", true);
        if (flapping)
            StopFlapping();
    }

    public void StopDive()
    {
        diving = false;
        animator.SetBool("dyker", false);

        float magnitudeBefore = velocity.magnitude;
        
        if (diveBoostTimer <= 0)
        {
            diveBoostTimer = diveBoostCooldown;
            velocity += forwardVector * diveBoostSpeed;
        }

        if (velocity.magnitude < magnitudeBefore * .5f)
            velocity += forwardVector * magnitudeBefore * .5f;

    }

    /// <summary>
    /// </summary>
    /// <param name="direction"> -1 = Clockwise, 0 = no rotation, +1 = Counter clockwise </param>
    public void Steer(sbyte direction)
    {
        rotationDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        float dT = Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z))//start flapping
            StartFlapping();
        else if (Input.GetKeyUp(KeyCode.Z))
            StopFlapping();
        if (Input.GetKeyDown(KeyCode.X))//start diving
            StartDive();
        else if (Input.GetKeyUp(KeyCode.X))
            StopDive();

        if (Input.GetKey(KeyCode.LeftArrow))
            Steer(1);
        else if (Input.GetKey(KeyCode.RightArrow))
            Steer(-1);

        if (currentEnergy < maxEnergy && !flapping)
            currentEnergy += energyRegeneration * dT;

        if (diving)
            currentDrag = diveDrag;
        else
            currentDrag = wingDrag;

        if (diveBoostTimer > 0)
            diveBoostTimer -= dT;

        if (flapTimer > 0)
            flapTimer -= dT;

        //rotation

        transform.Rotate(new Vector3(0, 0, 1), rotationCoefficient*rotationSpeed * rotationDirection * dT);

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z % 360);

        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z <= 270)
        {
            facingRight = false;
            if (!spriteRenderer.flipY)
                spriteRenderer.flipY = true;
        }
        else
        {
            facingRight = true;
            if (spriteRenderer.flipY)
                spriteRenderer.flipY = false;
        }

        if (!diving)
        {
            //angle velocity where bird is pointing
            velocity = (fwdMomentum * velocity + turnMomentum * velocity.magnitude * forwardVector)/(fwdMomentum + turnMomentum);

           

            if(velocity.magnitude < minSpeedAutoTurningThreshold && !(transform.eulerAngles.z > 270 - autoTurningMinAngleThreshold && transform.eulerAngles.z < 270 + autoTurningMinAngleThreshold))
            {
                float autoTurningMagnitude = 1 - (velocity.magnitude / minSpeedAutoTurningThreshold);

                Debug.Log(autoTurningMagnitude);

                if (facingRight)
                    autoTurningMagnitude *= -1;

                transform.Rotate(0, 0, autoTurnMaxStrength * autoTurningMagnitude * dT);
            }

            //apply gravity-forces
            velocity += forwardVector * -gravity * forwardVector.y * dT;
        }
        else if (diving)
            velocity.y -= gravity * dT;


        if (flapping)
        {
            currentEnergy -= flappingEnergyCost * dT;

            velocity += forwardVector * flappingAcceleration * dT;

            if (currentEnergy <= 0)
            {
                StopFlapping();
            }
        }

        //apply drag, exponential to limit maxspeed
        velocity -= (velocity * currentDrag) * (velocity.magnitude * currentDrag) * dT;

        //move the transform by velocity
        transform.position += velocity * dT;

        //reset rotation
        rotationDirection = 0;

        cameraGO.transform.localRotation = new Quaternion(0, 0, transform.rotation.z, -transform.rotation.w);


        #region debug

        if (DEBUG_ARROWS)
        {
            Debug.DrawLine(transform.position, transform.position + forwardVector, Color.red);
            Debug.DrawLine(transform.position, transform.position + upVector, Color.blue);
        }


        #endregion
    }
}
