using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roboboy : Creature
{
    // Start is called before the first frame update
    new void Start()
    {
        Lives = 2;
        base.Start();
    }

    // Update is called once per frame
    public override void TakeDamage()
    {
        FindObjectOfType<SoundSystem>().RoboDamaged();
        base.TakeDamage();
    }
}
