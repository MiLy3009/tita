using UnityEngine;
using System.Collections;

public class DesaparecerAlPresionar : MonoBehaviour
{
    private Animator animator;
    private Coroutine coroutineActual;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("No se encontró un Animator en: " + gameObject.name);
    }

    public void Desaparecer()
    {
        if (animator == null) animator = GetComponent<Animator>();

        if (animator != null)
        {
            if (coroutineActual != null) StopCoroutine(coroutineActual);
            animator.Play("Desaparecen");
            coroutineActual = StartCoroutine(DesactivarAlTerminar());
        }
    }

    private IEnumerator DesactivarAlTerminar()
    {
        yield return null; // espera un frame para que la animación arranque
        float duracion = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracion);
        gameObject.SetActive(false);
    }

    public void Aparecer()
    {
        if (coroutineActual != null) StopCoroutine(coroutineActual);

        gameObject.SetActive(true);

        // Re-obtener el animator SIEMPRE al aparecer
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            coroutineActual = StartCoroutine(ReproducirAnimacionAparece());
        }
        else
        {
            Debug.LogError("Animator no encontrado al Aparecer en: " + gameObject.name);
        }
    }

    private IEnumerator ReproducirAnimacionAparece()
    {
        yield return null; // espera que Unity reactive el objeto completamente
        animator.Rebind();
        animator.Update(0f);
        animator.Play("Aparecer");
    }
}