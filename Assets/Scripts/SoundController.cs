using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public List<AudioClip> noteSounds = new List<AudioClip>();

    void Awake()
    {
        MIDIController.NoteOn += PlaySound;
    }

    public void PlaySound(int note)
    {
        //cache previous soundcontroller object - is it playing, if not then use it OR make another (pool them)
        GameObject soundGameObject = new GameObject("SoundController");
        DestroyOverTime destroyOverTime =
            soundGameObject.AddComponent<DestroyOverTime>();
        AudioClip noteSound = noteSounds[note];
        if (noteSound != null) destroyOverTime.lifeTime = noteSound.length / 3;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        if (noteSound != null) audioSource.PlayOneShot(noteSound);
    }
}
