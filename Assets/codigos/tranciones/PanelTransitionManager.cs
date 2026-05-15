using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelTransitionManager : MonoBehaviour
{
    [Header("Configuración")]
    public float duracion = 0.4f;

    [Header("Panel Negro (asígnalo manualmente en el Inspector)")]
    public GameObject panelNegro;

    private CanvasGroup canvasGrupoNegro;
    private static PanelTransitionManager instancia;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        instancia = this;
        DontDestroyOnLoad(gameObject);

        InicializarPanelNegro();

        SceneManager.sceneLoaded += OnScenaCargada;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnScenaCargada;
    }

    void InicializarPanelNegro()
    {
        if (panelNegro == null)
            panelNegro = CrearPanelNegro();

        canvasGrupoNegro = ObtenerCanvasGroup(panelNegro);
        canvasGrupoNegro.alpha = 1f;
        canvasGrupoNegro.blocksRaycasts = true;
        panelNegro.SetActive(true);
    }

    public void CambiarPanel(GameObject panelActual, GameObject panelNuevo)
    {
        StartCoroutine(Transicion(panelActual, panelNuevo));
    }

    private IEnumerator Transicion(GameObject actual, GameObject nuevo)
    {
        if (actual != null && actual.activeSelf)
        {
            CanvasGroup cg = ObtenerCanvasGroup(actual);
            cg.alpha = 1f; // reset por si quedó sucio
            yield return StartCoroutine(FadePanel(actual, 1f, 0f));
        }

        if (actual != null)
            actual.SetActive(false);

        if (nuevo != null)
        {
            nuevo.SetActive(true);
            SetAlpha(nuevo, 0f);
            yield return StartCoroutine(FadePanel(nuevo, 0f, 1f));
        }
    }

    public void CambiarEscena(string nombreEscena)
    {
        StartCoroutine(TransicionEscena(nombreEscena));
    }

    public void CambiarEscenaPorIndice(int indice)
    {
        StartCoroutine(TransicionEscenaIndice(indice));
    }

    private IEnumerator TransicionEscena(string nombreEscena)
    {
        panelNegro.SetActive(true);
        canvasGrupoNegro.blocksRaycasts = true;
        yield return StartCoroutine(FadeNegro(0f, 1f));

        AsyncOperation op = SceneManager.LoadSceneAsync(nombreEscena);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;

        yield return null;
        yield return null;

        yield return StartCoroutine(FadeNegro(1f, 0f));
        canvasGrupoNegro.blocksRaycasts = false;
    }

    private IEnumerator TransicionEscenaIndice(int indice)
    {
        panelNegro.SetActive(true);
        canvasGrupoNegro.blocksRaycasts = true;
        yield return StartCoroutine(FadeNegro(0f, 1f));

        AsyncOperation op = SceneManager.LoadSceneAsync(indice);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;

        yield return null;
        yield return null;

        yield return StartCoroutine(FadeNegro(1f, 0f));
        canvasGrupoNegro.blocksRaycasts = false;
    }

    private void OnScenaCargada(Scene escena, LoadSceneMode modo) { }

    private IEnumerator FadeNegro(float desde, float hasta)
    {
        canvasGrupoNegro.alpha = desde;
        float tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            canvasGrupoNegro.alpha = Mathf.Lerp(desde, hasta, tiempo / duracion);
            yield return null;
        }
        canvasGrupoNegro.alpha = hasta;
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
            cg = panel.AddComponent<CanvasGroup>();
        return cg;
    }

    private GameObject CrearPanelNegro()
    {
        Canvas canvas = Object.FindAnyObjectByType<Canvas>();

        GameObject panel = new GameObject("PanelFondoNegro");
        panel.transform.SetParent(canvas.transform, false);

        Image img = panel.AddComponent<Image>();
        img.color = Color.black;

        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        panel.transform.SetAsLastSibling();

        return panel;
    }
}