using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : Creature
{
    // Start is called before the first frame update
    new void Start()
    {
        Lives = 3;
        base.Start();
    }

    // Update is called once per frame
    public override void TakeDamage()
    {
        FindObjectOfType<SoundSystem>().CrabDamaged();
        base.TakeDamage();
    }
}
