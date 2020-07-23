using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField]
    int lives;

    [SerializeField]
    ParticleSystem onDieEffect;
    protected int Lives
    {
        get { return lives;}
        set
        {
            if (value <= 0)
            {
                GetDead();
                return;
            } else
            {
                lives = value;
            }

        }
    }

    protected LevelGenerator level;

    protected void Start()
    {
        level = FindObjectOfType<LevelGenerator>();
    }
    public virtual void TakeDamage()
    {
        Lives--;
    }
    
    protected virtual void GetDead()
    {
        level.creatures.Remove(transform);
        Instantiate(onDieEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
