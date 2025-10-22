using UnityEngine;

public class HabilidadPlayer : MonoBehaviour
{
    public GameObject TBala;
    public GameObject Derr_objeto;
    public HabilidadMoverObjeto hab_MovObj;
    public HabilidadRetrocederposicion hab_RetroPos;


    private BulletTimeController tb;
    private Derrumbe_objeto der_obj;


    private void IniciarMetodo()
    {
        tb = TBala.GetComponent<BulletTimeController>();
        der_obj = Derr_objeto.GetComponent<Derrumbe_objeto>();
    }
    public void EquiparArma(int nuevaArma)
    {
        if (nuevaArma == 0)
        {
            Debug.Log("Varita equipada");
            IniciarMetodo();
            tb.enabled = true;
            hab_MovObj.enabled = true;
            

            der_obj.enabled = false;
            hab_RetroPos.enabled = false;
        }

        else if (nuevaArma == 1)
        {
            Debug.Log("Cetro equipado");
            IniciarMetodo();
            tb.enabled = false;
            hab_MovObj.enabled = false;


            der_obj.enabled = true;
            hab_RetroPos.enabled = true;
        }
    }
}
