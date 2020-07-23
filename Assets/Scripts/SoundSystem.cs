using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField]
    AudioClip[] sounds;

    AudioSource audios;

    void Start()
    {
        audios = GetComponent<AudioSource>();
    }
    public void PlayTimeCreatureDead()
    {
        audios.clip = sounds[0];
        audios.Play();
    }

    public void PlayLooseLife()
    {
        audios.clip = sounds[1];
        audios.Play();
    }

    public void RoboDamaged()
    {

    }
}
