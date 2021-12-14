using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class JugadorNecesidades : MonoBehaviour, IRecibeDanyo
{
    //Necesidades:
    public Necesidad vida;
    public Necesidad hambre;
    public Necesidad sed;
    public Necesidad dormir;

    //Cantidad de vida por tiempo que pierdo, si tengo hambre o sed
    public float noComidaCantidadVidaQueDecae;
    public float noAguaCantidadVidaQueDecae;

    public UnityEvent alRecibirDanyo;

    private void Start()
    {
        vida.valorActual = vida.valorComienzo;
        hambre.valorActual = hambre.valorComienzo;
        sed.valorActual = sed.valorComienzo;
        dormir.valorActual = dormir.valorComienzo;
    }

    private void Update()
    {
        //Perdida por tiempo
        hambre.Restar(hambre.ratioPerdida * Time.deltaTime);
        sed.Restar(sed.ratioPerdida * Time.deltaTime);
        dormir.Anyadir(dormir.ratioRegeneracion * Time.deltaTime);
        
        //Nos quitamos vida
        if (hambre.valorActual == 0.0f)
        {
            vida.Restar(noComidaCantidadVidaQueDecae * Time.deltaTime);
        }

        if (sed.valorActual == 0.0f)
        {
            vida.Restar(noAguaCantidadVidaQueDecae * Time.deltaTime);
        }
        
        //Una vez hechos los calculos updateamos las barras de la interfaz
        vida.barraUI.fillAmount = vida.GetPorcentaje();
        sed.barraUI.fillAmount = sed.GetPorcentaje();
        hambre.barraUI.fillAmount = hambre.GetPorcentaje();
        dormir.barraUI.fillAmount = dormir.GetPorcentaje();

        if (vida.valorActual == 0.0f)
        {
            Morir();
        }
    }
    //Bloque de funciones basicas
    public void Curar(float cantidad)
    {
        vida.Anyadir(cantidad);
    }
    public void Comer(float cantidad)
    {
        hambre.Anyadir(cantidad);
    }
    public void Beber(float cantidad)
    {
        sed.Anyadir(cantidad);
    }
    public void Dormir(float cantidad)
    {
        dormir.Restar(cantidad);
    }
    
    //Recibir daño
    public void RecibirDanyo(int cantidad)
    {
        vida.Restar(cantidad);
        alRecibirDanyo?.Invoke();
    }
    //Morir
    public void Morir()
    {
        Debug.Log("Estoy muerto");
    }
    
}//fin clase

[System.Serializable]
public class Necesidad
{
    [HideInInspector]
    public float valorActual;
    public float valorMaximo;
    public float valorComienzo;
    public float ratioRegeneracion;
    public float ratioPerdida;
    public Image barraUI;
    
    //Añadir a la necesidad
    public void Anyadir(float cantidad)
    {
        valorActual = Mathf.Min(valorActual + cantidad, valorMaximo);
    }
    
    //Restar a la necesidad
    public void Restar(float cantidad)
    {
        valorActual = Mathf.Max(valorActual - cantidad, 0.0f);
    }
    
    //Devolver valor actual en porcentaje (0.0 - 1.0)
    public float GetPorcentaje()
    {
        return valorActual / valorMaximo;
    }
}// cierre

public interface IRecibeDanyo
{
    void RecibirDanyo(int cantidad);
}
