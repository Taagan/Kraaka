using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDove : Enemy
{
    public GameObject AggroRange;



    public override void Attack()
    {
        
    }

    public override void Start()
    {
        speed = 35.0f;
    }

    public override void Update()
    {
        if (currentTarget != null)
        {
            Debug.Log("2");

            if (Vector3.Distance(this.transform.position,currentTarget.transform.position) < 30f)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget.transform.position, speed * Time.deltaTime);
                //Vector3 targetDirection = currentTarget.transform.position - this.transform.position;

                //Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, 1f, 0.0f);
                Vector3 difference = currentTarget.transform.position - this.transform.position;

                float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

                this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
                Debug.Log("b");

            }
            else if (Vector3.Distance(this.transform.position, currentTarget.transform.position) > 30f)
            {
                currentTarget = null;
            }
        }
    }


}
