using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //player animator
    public Animator Anim;
    void Awake()
    {
        Anim = GetComponent<Animator>();
    }
    public void ResetAnimation(){
        Anim.SetBool("isLookUp", false);
        Anim.SetBool("isRun", false);
    }
    public void Run()
    {
        Anim.SetBool("isRun", true);
    }


}