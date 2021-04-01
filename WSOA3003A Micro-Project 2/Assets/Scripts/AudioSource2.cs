using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSource2 : MonoBehaviour
{
    public AudioSource defenseSound;

    private void Start()
    {
        defenseSound = GetComponent<AudioSource>();
    }
}
