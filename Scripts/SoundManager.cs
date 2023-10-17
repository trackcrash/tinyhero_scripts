using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //터치시 효과음
    public AudioSource buttonSound;
    
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            buttonSound.Play();
        } 
    }
}
