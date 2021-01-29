using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    protected int hp;
    protected float speed;
    public GameObject currentTarget;

    public abstract void  Attack();
    public abstract void Update();
    public abstract void Start();


}
