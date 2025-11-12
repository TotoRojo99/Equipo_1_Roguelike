using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTimeController : MonoBehaviour
{
    [Header("Configuración")]
    public Key activarTecla = Key.Space; // Nuevo Input System usa 'Key' en vez de KeyCode
    public float duracion = 5f;
    public float factorRalentizacion = 0.5f;
    public float cooldown = 15f;

    private bool enBulletTime = false;
    private float proximoUso = 0f;
    public AudioSource tictac;

    [Header("Partículas")]
    public ParticleSystem particles;

    private InputAction accionActivar;

    void OnEnable()
    {
        // Creamos la acción para la tecla
        accionActivar = new InputAction(
            "ActivarBulletTime",
            InputActionType.Button,
            "<Keyboard>/" + activarTecla.ToString()
        );

        // Callback cuando se presiona
        accionActivar.performed += ctx => IntentarActivarBulletTime();

        accionActivar.Enable();
    }

    void OnDisable()
    {
        accionActivar.Disable();
        accionActivar.performed -= ctx => IntentarActivarBulletTime();
    }

    void IntentarActivarBulletTime()
    {
        if (!enBulletTime && Time.time >= proximoUso)
        {
            StartCoroutine(ActivarBulletTime());
            
            tictac.Play();
        }
    }

    IEnumerator ActivarBulletTime()
    {
        enBulletTime = true;
        proximoUso = Time.time + cooldown;

        if (particles != null)
        {
            particles.Play();
        }

        
        // Activamos partículas durante el cooldown


        // Activamos bullet time
        Time.timeScale = factorRalentizacion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duracion);

        // Volvemos todo a la normalidad
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        enBulletTime = false;
        tictac.Stop();
        if (particles != null)
        {
            particles.Stop();
        }
        // Esperamos el resto del cooldown para apagar partículas
        float tiempoRestante = proximoUso - Time.time;
        if (tiempoRestante > 0)
            yield return new WaitForSeconds(tiempoRestante);

        
    }
}