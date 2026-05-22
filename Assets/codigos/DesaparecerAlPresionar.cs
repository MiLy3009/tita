using UnityEngine;
using System.Collections;

public class DesaparecerAlPresionar : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("No se encontró un Animator en: " + gameObject.name);
    }

    public void Desaparecer()
    {
        if (animator != null)
        {
            animator.Play("Desaparecen");
            StartCoroutine(DesactivarAlTerminar());
        }
    }

    private IEnumerator DesactivarAlTerminar()
    {
        yield return null;
        float duracion = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracion);
        gameObject.SetActive(false);
    }

    public void Aparecer()
    {
        gameObject.SetActive(true);
        StartCoroutine(ReproducirAnimacionAparece());
    }

    private IEnumerator ReproducirAnimacionAparece()
    {
        yield return null;
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
            animator.Play("Aparecer");
        }
    }
}