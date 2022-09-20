using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume)
    {
        //create
        GameObject audioObject = new GameObject("Audio2D");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        //configure
        audioSource.clip = clip;
        audioSource.volume = volume;
        //activate
        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        //return in case other things need it
        return audioSource;
    }

    public static AudioSource PlayClip3D(AudioClip clip, float volume, Vector3 position)
    {
        //create
        GameObject audioObject = new GameObject("Audio3D");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        //configure
        audioSource.spatialBlend = 1f;
        audioSource.clip = clip;
        audioSource.volume = volume;
        //move the audio object to where it should be played
        audioObject.transform.Translate(position, Space.World);
        //activate
        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        //return in case other things need it
        return audioSource;
    }
}
