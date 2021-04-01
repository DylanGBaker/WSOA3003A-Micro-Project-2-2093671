using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSource1 : MonoBehaviour
{
    public AudioSource swordUnsheathingsound;

    private void Start()
    {
        swordUnsheathingsound = GetComponent<AudioSource>();
    }
}
