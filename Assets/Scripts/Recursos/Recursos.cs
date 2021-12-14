using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursos : MonoBehaviour
{
    // itemdata del objeto que vayamos a obtener
    public ItemData itemARecibir;
    public int cantidadPorHit = 1;
    //Capacidad antes de romperse
    public int capacidad;
    //particulas de golpeo
    public GameObject particula;

    public void Recolectar(Vector3 hitPoint, Vector3 hitNormal)
    {
        //Darle al player la cantidad por hit
        for (int i = 0; i < cantidadPorHit; i++)
        {
            if (capacidad <= 0)
            {
                break;
            }

            capacidad -= 1;
            Inventario.instancia.AddItem(itemARecibir);
        }
        //Creamos la particula de golpeo
        Destroy(Instantiate(particula, hitPoint, Quaternion.LookRotation(hitNormal, Vector3.up)), 1.0f);
        //Si se vacian los destruimos
        if (capacidad <= 0)
        {
            Destroy(gameObject);
        }
    }
}
