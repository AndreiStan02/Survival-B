using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    // Variables
    //El tiempo va a ser de 0 a 1, siendo 0 las 12 am y 1 las 11.59 am
    [Range(0.0f, 1.0f)] 
    public float tiempo;
    public float duracionDia;
    public float horaInicio = 0.4f;
    public float ratioTiempo;
    public Vector3 mediodia;

    [Header("Sol")] 
    public Light sol;
    public Gradient colorSol;
    public AnimationCurve intensidadSol;

    [Header("Luna")] 
    public Light luna;
    public Gradient colorLuna;
    public AnimationCurve intensidadLuna;

    [Header("Otros")]
    public AnimationCurve intensidadGlobal;
    public AnimationCurve reflejosGlobales;

    private void Start()
    {
        ratioTiempo = 1.0f / duracionDia;
        tiempo = horaInicio;
    }

    private void Update()
    {
        // Incrementar tiempo
        tiempo += ratioTiempo * Time.deltaTime;
        // El tiempo tene q hacer un loop
        if (tiempo >= 1.0f)
        {
            tiempo = 0.0f;
        }
        // Rotacion sol y luna
        sol.transform.eulerAngles = (tiempo - 0.25f) * mediodia * 4.0f;
        luna.transform.eulerAngles = (tiempo - 0.25f) * mediodia * 4.0f;
        // Intensidades
        sol.intensity = intensidadSol.Evaluate(tiempo);
        luna.intensity = intensidadLuna.Evaluate(tiempo);
        // Color
        sol.color = colorSol.Evaluate(tiempo);
        luna.color = colorLuna.Evaluate(tiempo);
        // Activar/desactivar luces
        if (sol.intensity == 0 && sol.gameObject.activeInHierarchy)
        {
            sol.gameObject.SetActive(false);
        }
        else if (sol.intensity > 0 && !sol.gameObject.activeInHierarchy)
        {
            sol.gameObject.SetActive(true);
        }
        
        if (luna.intensity == 0 && luna.gameObject.activeInHierarchy)
        {
            luna.gameObject.SetActive(false);
        }
        else if (luna.intensity > 0 && !luna.gameObject.activeInHierarchy)
        {
            luna.gameObject.SetActive(true);
        }
        
        // Intensidad y reflejos globales
        RenderSettings.ambientIntensity = intensidadGlobal.Evaluate(tiempo);
        RenderSettings.reflectionIntensity = reflejosGlobales.Evaluate(tiempo);
    }
}
