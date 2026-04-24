using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PanelTransitionManager : MonoBehaviour
{
    [Header("Configuración")]
    public float duracion = 0.4f;

    // Llama este método desde el botón
    public void CambiarPanel(GameObject panelActual, GameObject panelNuevo)
    {
        StartCoroutine(Transicion(panelActual, panelNuevo));
    }

    private IEnumerator Transicion(GameObject actual, GameObject nuevo)
    {
        // Fade OUT del panel actual
        yield return StartCoroutine(FadePanel(actual, 1f, 0f));
        actual.SetActive(false);

        // Activar y hacer Fade IN del nuevo panel
        nuevo.SetActive(true);
        SetAlpha(nuevo, 0f);
        yield return StartCoroutine(FadePanel(nuevo, 0f, 1f));
    }

    private IEnumerator FadePanel(GameObject panel, float desde, float hasta)
    {
        CanvasGroup cg = ObtenerCanvasGroup(panel);
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            cg.alpha = Mathf.Lerp(desde, hasta, tiempo / duracion);
            yield return null;
        }

        cg.alpha = hasta;
    }

    private void SetAlpha(GameObject panel, float alpha)
    {
        ObtenerCanvasGroup(panel).alpha = alpha;
    }

    private CanvasGroup ObtenerCanvasGroup(GameObject panel)
    {
        CanvasGroup cg = panel.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = panel.AddComponent<CanvasGroup>(); // Lo agrega automáticamente si no existe
        return cg;
    }
}