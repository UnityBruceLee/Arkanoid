using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : Creature
{
    // Start is called before the first frame update
    new void Start()
    {
        Lives = 4;
        base.Start();
    }

    // Update is called once per frame
    public override void TakeDamage()
    {
        FindObjectOfType<SoundSystem>().QuestionDamaged();
        base.TakeDamage();
    }
}
