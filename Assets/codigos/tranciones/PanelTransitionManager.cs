using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelTransitionManager : MonoBehaviour
{
    [Header("Configuración")]
    public float duracion = 0.4f;

    [Header("Transición de Escena")]
    public GameObject panelNegro; // Un panel negro que cubre toda la pantalla

    private CanvasGroup canvasGrupoNegro;

    void Awake()
    {
        // Si no asignaste el panel negro, lo crea automáticamente
        if (panelNegro == null)
        {
            panelNegro = CrearPanelNegro();
        }

        canvasGrupoNegro = ObtenerCanvasGroup(panelNegro);
        canvasGrupoNegro.alpha = 0f;
        panelNegro.SetActive(false);

        // Marca este objeto para que no se destruya al cambiar escena
        DontDestroyOnLoad(gameObject);

        // Escucha cuando una escena termina de cargar para hacer fade in
        SceneManager.sceneLoaded += OnScenaCargada;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnScenaCargada;
    }

    // ─── Transición entre paneles (mismo comportamiento que antes) ───────────

    public void CambiarPanel(GameObject panelActual, GameObject panelNuevo)
    {
        StartCoroutine(Transicion(panelActual, panelNuevo));
    }

    private IEnumerator Transicion(GameObject actual, GameObject nuevo)
    {
        yield return StartCoroutine(FadePanel(actual, 1f, 0f));
        actual.SetActive(false);

        nuevo.SetActive(true);
        SetAlpha(nuevo, 0f);
        yield return StartCoroutine(FadePanel(nuevo, 0f, 1f));
    }

    // ─── Transición de escena con negro ─────────────────────────────────────

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(TransicionEscena(nombreEscena));
    }

    public void CambiarEscenaPorIndice(int indice)
    {
        StartCoroutine(TransicionEscena(indice));
    }

    private IEnumerator TransicionEscena(object escena)
    {
        // Fade a negro
        panelNegro.SetActive(true);
        yield return StartCoroutine(FadeNegro(0f, 1f));

        // Carga la escena
        if (escena is string nombre)
            SceneManager.LoadScene(nombre);
        else if (escena is int indice)
            SceneManager.LoadScene(indice);
    }

    private void OnScenaCargada(Scene escena, LoadSceneMode modo)
    {
        // Cuando la nueva escena cargó, hace fade de negro a transparente
        StartCoroutine(FadeNegroDespuesDeCargar());
    }

    private IEnumerator FadeNegroDespuesDeCargar()
    {
        // Espera un frame para que la escena termine de inicializarse
        yield return null;

        yield return StartCoroutine(FadeNegro(1f, 0f));
        panelNegro.SetActive(false);
    }

    private IEnumerator FadeNegro(float desde, float hasta)
    {
        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            canvasGrupoNegro.alpha = Mathf.Lerp(desde, hasta, tiempo / duracion);
            yield return null;
        }
        canvasGrupoNegro.alpha = hasta;
    }

    // ─── Utilidades ──────────────────────────────────────────────────────────

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
            cg = panel.AddComponent<CanvasGroup>();
        return cg;
    }

    // Crea automáticamente el panel negro si no existe en escena
    private GameObject CrearPanelNegro()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        GameObject panel = new GameObject("PanelNegro");
        panel.transform.SetParent(canvas.transform, false);

        Image img = panel.AddComponent<Image>();
        img.color = Color.black;

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Se asegura de que esté encima de todo
        panel.transform.SetAsLastSibling();

        return panel;
    }
}