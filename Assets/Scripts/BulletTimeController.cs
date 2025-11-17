using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTimeController : MonoBehaviour, IHabilidadConCooldown
{
    [Header("Configuración")]
    public Key activarTecla = Key.Space;
    public float duracion = 5f;
    public float factorRalentizacion = 0.5f;
    public float cooldown = 15f;

    private bool enBulletTime = false;
    private float proximoUso = 0f;
    private float cooldownRestante = 0f;

    public AudioSource tictac;
    public ParticleSystem particles;

    private InputAction accionActivar;

    void OnEnable()
    {
        accionActivar = new InputAction(
            "ActivarBulletTime",
            InputActionType.Button,
            "<Keyboard>/" + activarTecla.ToString()
        );

        accionActivar.performed += ctx => IntentarActivarBulletTime();
        accionActivar.Enable();
    }

    void OnDisable()
    {
        accionActivar.Disable();
        accionActivar.performed -= ctx => IntentarActivarBulletTime();
    }

    void Update()
    {
        if (cooldownRestante > 0)
            cooldownRestante -= Time.deltaTime;
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
        cooldownRestante = cooldown;

        if (particles != null)
            particles.Play();

        Time.timeScale = factorRalentizacion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duracion);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        enBulletTime = false;
        tictac.Stop();

        if (particles != null)
            particles.Stop();
    }

    // IMPLEMENTACIÓN UI
    public float CooldownRestante() => cooldownRestante;
    public float CooldownMaximo() => cooldown;
    public bool EnCooldown() => cooldownRestante > 0;
}