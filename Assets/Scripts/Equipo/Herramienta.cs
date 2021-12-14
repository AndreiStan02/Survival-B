using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herramienta : Equipo
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;

    [Header("Recoleccion")] 
    public bool puedeRecolectar;

    [Header("Combate")] 
    public bool doesDealDamage;
    public int damage;
    
    //Componentes
    private Animator anim;
    private Camera cam;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    private void PuedeAtacar()
    {
        attacking = false;
    }

    //Sobrescribir la funcion del padre OnAttackInput
    public override void OnAttackInput()
    {
        //Llamara al animator para que reproduzaca la animación
        if (!attacking)
        {
            attacking = true;
            anim.SetTrigger("Atacar");
            Invoke("PuedeAtacar", attackRate);
        }
    }

    public void Golpear()
    {
        
    }
}
