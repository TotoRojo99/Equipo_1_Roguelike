using UnityEngine;



[CreateAssetMenu(fileName = "NuevaArma", menuName = "Juego/Arma")]
public class WeaponData : ScriptableObject
{
    public string nombre;
    public Sprite icono;
    public MonoBehaviour[] habilidades; // scripts que implementen IHabilidad
}