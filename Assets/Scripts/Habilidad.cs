using UnityEngine;

public abstract class Habilidad : ScriptableObject
{
    public string nombre;
    public Sprite icono;

    public abstract void Activar(GameObject usuario); // el GameObject que usa la habilidad
}