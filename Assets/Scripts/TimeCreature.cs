using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCreature : Creature
{
    PlatformMover platform;

    // Start is called before the first frame update
    new void Start()
    {
        Lives = 1;
        platform = FindObjectOfType<PlatformMover>();
        StartCoroutine(CreatureAliver());
        base.Start();
    }

    IEnumerator CreatureAliver()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 12f));
            if (platform.state == PlatformMover.State.MovingLeft)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (platform.state == PlatformMover.State.MovingRight)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    protected override void GetDead()
    {
        FindObjectOfType<SoundSystem>().PlayTimeCreatureDead();
        base.GetDead();
    }
}
