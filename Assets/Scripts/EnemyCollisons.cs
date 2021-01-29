using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisons : MonoBehaviour
{
    public GameObject parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("t");

        if (collision.gameObject.tag == "Player")
        {
            parent.GetComponent<BombDove>().currentTarget = collision.gameObject;
            Debug.Log("a");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("l");

        if (collision.gameObject.tag == "Player")
        {
            parent.GetComponent<BombDove>().currentTarget = collision.gameObject;
            Debug.Log("c");
        }
    }
}
