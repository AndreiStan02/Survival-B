using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    //Atraves de un raycas para comprobar si tenemos delante un objeto con el que interactuar
    
    public float checkRate = 0.05f;
    //Momento en el que se hizo la ultima comprobacion
    private float ultimaVezComprobado;
    //Distancia a la que tiene que detectar
    public float maxDistacia;
    public LayerMask layer;

    private Camera cam;

    private GameObject objetoActual;
    private IInteractuable interactuableActual;

    public Text mensaje;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //Time.time -> Devuelve en segundos el tiempo pasado desde el frame
        //chequear el radio
        if (Time.time - ultimaVezComprobado > checkRate)
        {
            ultimaVezComprobado = Time.time;
            //Creamos el rayo, desde el centro de la pantalla
            Ray rayo = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            //Hemos chocado con algo?
            if (Physics.Raycast(rayo, out hit, maxDistacia, layer))
            {
                //El nuevo objeto es el mismo q el actual??
                if (hit.collider.gameObject != objetoActual)
                {
                    objetoActual = hit.collider.gameObject;
                    interactuableActual = hit.collider.GetComponent<IInteractuable>();
                    SetMensaje();
                }
            }
            else
            {
                objetoActual = null;
                interactuableActual = null;
                mensaje.gameObject.SetActive(false);
            }
        }
    }

    private void SetMensaje()
    {
        mensaje.gameObject.SetActive(true);
        mensaje.text = string.Format("<b> [E] </b> {0}", interactuableActual.GetMesajeInteraccion());
    }
    
    //Funcion que se queda a la espera de q presionemos "e" para interactuar
    public void AlInteractuar(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && interactuableActual != null)
        {
            interactuableActual.Interactuar();
            objetoActual = null;
            interactuableActual = null;
            mensaje.gameObject.SetActive(false);
        }
    }
}//F

public interface IInteractuable
{
    string GetMesajeInteraccion();

    void Interactuar();
}