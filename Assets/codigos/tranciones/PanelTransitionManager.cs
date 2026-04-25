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
        // Singleton: evita duplicados al cambiar de escena
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
        {
            panelNegro = CrearPanelNegro();
        }

        canvasGrupoNegro = ObtenerCanvasGroup(panelNegro);

        // Empieza completamente negro y opaco para que no se vea nada raro al inicio
        canvasGrupoNegro.alpha = 1f;
        canvasGrupoNegro.blocksRaycasts = true;
        panelNegro.SetActive(true);
    }

    // ─── Transición entre paneles (misma escena) ─────────────────────────────

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

    // ─── Transición de escena con negro ──────────────────────────────────────

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
        // 1. Fade a negro
        panelNegro.SetActive(true);
        canvasGrupoNegro.blocksRaycasts = true;
        yield return StartCoroutine(FadeNegro(0f, 1f));

        // 2. Carga la escena en segundo plano pero NO la activa todavía
        AsyncOperation op = SceneManager.LoadSceneAsync(nombreEscena);
        op.allowSceneActivation = false;

        // Espera a que cargue al 90% (Unity para en 0.9 hasta allowSceneActivation)
        while (op.progress < 0.9f)
            yield return null;

        // 3. Activa la escena (aquí cambia, pero la pantalla sigue negra)
        op.allowSceneActivation = true;

        // Espera un frame para que la escena termine de inicializarse
        yield return null;
        yield return null;

        // 4. Fade de negro a transparente
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

    // El fade de entrada al cargar escena ya no es necesario aquí
    // porque lo manejamos con AsyncOperation arriba
    private void OnScenaCargada(Scene escena, LoadSceneMode modo)
    {
        // Nada — el fade de entrada lo controla TransicionEscena directamente
    }

    // ─── Fades ───────────────────────────────────────────────────────────────

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

    // ─── Crear panel negro automático ────────────────────────────────────────

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