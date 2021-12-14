using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus : MonoBehaviour
{
    public int danyo;
    //Mientras estemos en colision con el cactus
    //Numero de golpes por segundo
    public float ratioDanyo;
    private List<IRecibeDanyo> cosasADanyar = new List<IRecibeDanyo>();

    private void Start()
    {
        StartCoroutine(RepartirDanyo());
    }

    IEnumerator RepartirDanyo()
    {
        while (true)
        {
            for (int i = 0; i < cosasADanyar.Count; i++)
            {
                cosasADanyar[i].RecibirDanyo(danyo);
            }
            yield return new WaitForSeconds(ratioDanyo);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<IRecibeDanyo>() != null)
        {
            cosasADanyar.Add(other.gameObject.GetComponent<IRecibeDanyo>());
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<IRecibeDanyo>() != null)
        {
            cosasADanyar.Remove(other.gameObject.GetComponent<IRecibeDanyo>());
        }
    }
}
