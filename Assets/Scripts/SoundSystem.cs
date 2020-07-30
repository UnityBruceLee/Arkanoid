using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name == "3")
        {
            audios.clip = sounds[5];
            audios.Play();
            return;
        }
        audios.clip = sounds[1];
        audios.Play();
    }

    public void RoboDamaged()
    {
        audios.clip = sounds[2];
        audios.Play();
    }
    public void CrabDamaged()
    {
        audios.clip = sounds[3];
        audios.Play();
    }

    public void QuestionDamaged()
    {
        audios.clip = sounds[4];
        audios.Play();
    }
}
