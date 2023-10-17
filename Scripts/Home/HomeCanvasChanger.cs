using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCanvasChanger : MonoBehaviour
{
    public GameObject[] Canvas;

    public void ChangeCanvas(int index)
    {
        for (int i = 0; i < Canvas.Length; i++)
        {
            if (i == index)
            {
                Canvas[i].SetActive(true);
            }
            else
            {
                Canvas[i].SetActive(false);
            }
        }
    }
}
