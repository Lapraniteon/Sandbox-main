using System;
using UnityEngine;

public class ScreamBehaviour : AttributeBehaviour
{

    [SerializeField]
    private AudioSource _screamAudio;
    [SerializeField]
    private AudioClip _screamClip;
    
    private void ParentOnCollisionEnter()
    {
        _screamAudio.PlayOneShot(_screamClip);
    }
}
