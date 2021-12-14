using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipoManager : MonoBehaviour
{
    // Necesitamos el equipo actual
    public Equipo equipoActual;
    public Transform parentEquipo;

    public static EquipoManager instancia;

    private PlayerController controller;

    private void Awake()
    {
        instancia = this;
        controller = GetComponent<PlayerController>();
    }
    
    //Recoger el click del raton
    public void ClickNormal(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && equipoActual != null && controller.puedeMirar == true)
        {
            equipoActual.OnAttackInput();
        }
    }
    
    //Ataque alt
    public void ClickAlt(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && equipoActual != null && controller.puedeMirar == true)
        {
            equipoActual.OnAttackAltInput();
        }
    }
    
    //se llama al equipar
    public void Equipar(ItemData item)
    {
        Desequipar();
        equipoActual = Instantiate(item.equipoPrefab, parentEquipo).GetComponent<Equipo>();
    }
    
    //Se llama al desequipar
    public void Desequipar()
    {
        if (equipoActual != null)
        {
            Destroy(equipoActual.gameObject);
            equipoActual = null;
        }
    }

}//fin de la clase principal
