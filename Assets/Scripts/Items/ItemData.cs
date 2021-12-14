using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item",menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")] 
    public string nombre;
    public string descripcion;
    public Sprite icono;
    public GameObject prefabDrop;
    public ItemType tipo;

    [Header("Stackable")]
    public bool puedeStackear;
    public int maxNumStack;

    [Header("Consumible")]
    public ItemDataConsumible[] stats;

    [Header("PrefabEquipable")] 
    public GameObject equipoPrefab;


}

public enum ItemType
{
    Recurso,
    Equipable,
    Consumible
}

public enum TipoStatConsumible
{
    Hambre,
    Sed,
    Vida,
    Sueño
}

[System.Serializable]
public class ItemDataConsumible
{
    public TipoStatConsumible tipo;
    public float valor;
}