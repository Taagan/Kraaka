using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    protected int hp;
    protected float speed;
    protected GameObject currentTarget;

    public abstract void  Attack();
    public abstract void Update();
    public abstract void Start();


}
