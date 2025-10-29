using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
    
{
    public int ArmaElegida;

    public GameObject TBala;
    public GameObject Derr_objeto;
    public GameObject Derr_objeto1;
    public GameObject Derr_objeto2;
    public GameObject Derr_objeto3;
    public HabilidadMoverObjeto hab_MovObj;
    public HabilidadRetrocederposicion hab_RetroPos;

    private BulletTimeController tb;
    private Derrumbe_objeto der_obj;
    private Derrumbe_objeto der_obj1;
    private Derrumbe_objeto der_obj2;
    private Derrumbe_objeto der_obj3;


    private void IniciarMetodo()
    {
        tb = TBala.GetComponent<BulletTimeController>();
        der_obj = Derr_objeto.GetComponent<Derrumbe_objeto>();
        der_obj1 = Derr_objeto1.GetComponent<Derrumbe_objeto>();
        der_obj2 = Derr_objeto2.GetComponent<Derrumbe_objeto>();
        der_obj3 = Derr_objeto3.GetComponent<Derrumbe_objeto>();
    }
    public void EquiparArma(int nuevaArma)
    {
        ArmaElegida = nuevaArma;
    if (nuevaArma == 0)
        {
            Debug.Log("Varita equipada");
            IniciarMetodo();
            tb.enabled = true;
            hab_MovObj.enabled = true;

            der_obj.enabled = false;
            der_obj1.enabled = false;
            der_obj2.enabled = false;
            der_obj3.enabled = false;
            hab_RetroPos.enabled = false;
        }

        else if (nuevaArma == 1)
        {
            Debug.Log("Cetro equipado");
            IniciarMetodo();
            tb.enabled = false;
            hab_MovObj.enabled = false;
        
            der_obj.enabled = true;
            der_obj1.enabled = true;
            der_obj2.enabled = true;
            der_obj3.enabled = true;
            hab_RetroPos.enabled = true;
        }
    }
}
