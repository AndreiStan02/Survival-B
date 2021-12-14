using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Inventario : MonoBehaviour
{
    //Array de itemSlotUI- parte visual de el slot
    public ItemSlotUI[] uiSlots;
    //Array del slot fisico y sus datos (itemData ,cantidad..
    public ItemSlot[] slots;
    //El propio canvas del inventario
    public GameObject ventanaInventario;
    //Posicion de dropeo de objetos
    public Transform drop;

    [Header("Item Seleccionado")] 
    private ItemSlot itemSeleccionado;
    private int indiceItem;
    
    // Variables para controlar la parte derecha de nuestro inventario
    public Text nombreItem;
    public Text descripcionItem;
    public Text statItem;
    public Text statNombreItem;
    public GameObject botonUsar;
    public GameObject botonDrop;
    public GameObject botonEquipar;
    public GameObject botonDesequipar;
    // El indice del objeto que esta equipado
    private int itemEquipado;
    private PlayerController controller;
    private JugadorNecesidades necesidades;

    [Header("Eventos")]
    //Evento que "congela" el juego cuando abrimos el inventario
    public UnityEvent abrirInventario;
    public UnityEvent cerrarInventario;
    
    //Singleton de esta misma clase
    public static Inventario instancia;

    private void Awake()
    {
        //Inicializamos el singletin
        instancia = this;
        controller = GetComponent<PlayerController>();
        necesidades = GetComponent<JugadorNecesidades>();
    }

    private void Start()
    {
        //Escondemos el inventario
        ventanaInventario.SetActive(false);
        //Inicializamos los arrays de slots y slotsUI
        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].indice = i;
            uiSlots[i].LimpiarSlot();
            
        }

        LimpiarItemSeleccionado();
    }//Fin del start
    
    //Abrir y cerrar inventario
    public void Toggle()
    {
        if (ventanaInventario.activeInHierarchy)
        {
            ventanaInventario.SetActive(false);
            cerrarInventario.Invoke();
            controller.ActivarCursor(false);
        }
        else
        {
            ventanaInventario.SetActive(true);
            abrirInventario.Invoke();
            LimpiarItemSeleccionado();
            controller.ActivarCursor(true);
        }
    }

    public bool EstaAbierto()
    {
        return ventanaInventario.activeInHierarchy;
    }
    
    //Añadir objeto al inventario
    public void AddItem(ItemData item)
    {
        //Puede stackear
        if (item.puedeStackear)
        {
            ItemSlot slot = GetItemStack(item);
            if (slot != null)
            {
                //Coemos en el slot y aumentamos su cantidad
                slot.cantidad++;
                UpdateUI();
                return;
            }
        }
        //no puede stackear
        ItemSlot slotVacio = GetSlotVacio();
        if (slotVacio != null)
        {
            //Asignar el item a ese slot vacio
            slotVacio.item = item;
            slotVacio.cantidad = 1;
            UpdateUI();
            return;
        }

        TirarItem(item);
    }

    private void TirarItem(ItemData item)
    {
        Instantiate(item.prefabDrop, drop.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }
    
    //Devuelve el slot en el cual puede stackear el item q le pasemos 
    // en caso contrario devuelve null
    ItemSlot GetItemStack(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            //Si el slot tiene un itemData igual q el mio y su cantidad no sobrepasa el maxStack
            if (slots[i].item == item && slots[i].cantidad < item.maxNumStack)
            {
                return slots[i];
            }
        }
        return null;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                uiSlots[i].Set(slots[i]);
            }
            else
                uiSlots[i].LimpiarSlot();
            
        }
    }
    
    //Devolver un slot vacio si hay, si no quedan null
    private ItemSlot GetSlotVacio()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    private void LimpiarItemSeleccionado()
    {
        itemSeleccionado = null;
        nombreItem.text = string.Empty;
        descripcionItem.text = string.Empty;
        statNombreItem.text = string.Empty;
        statItem.text = string.Empty;
        botonDrop.SetActive(false);
        botonUsar.SetActive(false);
        botonEquipar.SetActive(false);
        botonDesequipar.SetActive(false);
    }
    
    //Metodo para seleccionar un item al hacer click en el slot
    public void SelectItem(int indice)
    {
        if (slots[indice].item == null)
        {
            return;
        }

        itemSeleccionado = slots[indice];
        indiceItem = indice;
        
        //Seteamos Texto
        nombreItem.text = itemSeleccionado.item.nombre;
        descripcionItem.text = itemSeleccionado.item.descripcion;

        statNombreItem.text = string.Empty;
        statItem.text = string.Empty;

        for (int i = 0; i < itemSeleccionado.item.stats.Length; i++)
        {
            statNombreItem.text += itemSeleccionado.item.stats[i].tipo.ToString() + "\n";
            statItem.text += itemSeleccionado.item.stats[i].valor.ToString() + "\n";

        }
        
        //Activar los botones correspondientes segun el tipo de objeto
        botonUsar.SetActive(itemSeleccionado.item.tipo == ItemType.Consumible);
        botonEquipar.SetActive(itemSeleccionado.item.tipo == ItemType.Equipable && !uiSlots[indice].equipado);
        botonDesequipar.SetActive(itemSeleccionado.item.tipo == ItemType.Equipable && uiSlots[indice].equipado);
        botonDrop.SetActive(true);
    }
    
    //Boton usar
    public void BotonUsar()
    {
        //si el objeto seleccionado es consumible
        if (itemSeleccionado.item.tipo == ItemType.Consumible)
        {
            for (int i = 0; i < itemSeleccionado.item.stats.Length; i++)
            {
               //Switch para cada caso de estado
               switch (itemSeleccionado.item.stats[i].tipo)
               {
                   case TipoStatConsumible.Hambre: 
                       necesidades.Comer(itemSeleccionado.item.stats[i].valor); 
                       break;
                   case TipoStatConsumible.Sed: 
                       necesidades.Beber(itemSeleccionado.item.stats[i].valor);
                       break;
                   case TipoStatConsumible.Sueño:
                       necesidades.Dormir(itemSeleccionado.item.stats[i].valor);
                       break;
                   case TipoStatConsumible.Vida: 
                       necesidades.Curar(itemSeleccionado.item.stats[i].valor);
                       break;
               }
            }
            EliminarItemSeleccionado();
        }
    }
    
    //boton Equipar
    public void BotonEquipar()
    {
        if (uiSlots[itemEquipado].equipado)
        {
            Desquipar(itemEquipado);
        }

        uiSlots[indiceItem].equipado = true;
        //indiceitem es el indice en el array del slot en el que hemos clickado
        //itemEquipado -> indice del item que tenemos equipado en este momento
        itemEquipado = indiceItem;
        EquipoManager.instancia.Equipar(itemSeleccionado.item);
        UpdateUI();
        SelectItem(indiceItem);
    }
    
    //boton Desequipar
    public void BotonDesequipar()
    {
        Desquipar(indiceItem);

    }
    
    //Desequipar
    private void Desquipar(int indice)
    {
        uiSlots[indice].equipado = false;
        EquipoManager.instancia.Desequipar();
        UpdateUI();
        if (indiceItem == indice)
        {
            SelectItem(indice);
        }
    }
    
    //boton Drop
    public void BotonDrop()
    {
        TirarItem(itemSeleccionado.item);
        EliminarItemSeleccionado();
    }

    private void EliminarItemSeleccionado()
    {
        itemSeleccionado.cantidad--;

        if (itemSeleccionado.cantidad == 0)
        {
            if (uiSlots[indiceItem].equipado == true)
            {
                Desquipar(indiceItem);
            }

            itemSeleccionado.item = null;
            LimpiarItemSeleccionado();
        }
        UpdateUI();
    }

    public void EliminarItem(ItemData item)
    {
        
    }
    
    //Tiene el jugador x cantidad de x item?
    public bool TieneItems(ItemData item, int cantidad)
    {
        return false;
    }

    //Input system
    public void BotonAbrirInventario(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }
    
}//fin clase principal

public class ItemSlot
{
    public ItemData item;
    public int cantidad;
}
