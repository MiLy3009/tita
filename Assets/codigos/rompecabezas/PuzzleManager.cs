using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [Header("Piezas del rompecabezas (en orden correcto: pieza1..pieza16)")]
    public List<PuzzlePiece> piezas = new List<PuzzlePiece>();

    [Header("Efectos (opcional)")]
    public AudioClip sonidoClick;
    public AudioClip sonidoCompletado;
    public GameObject panelVictoria;

    [Header("Panel del rompecabezas")]
    public GameObject panelRompecabezas;

    [Header("Panel a apagar al abrir")]
    public GameObject panelCelular;

    private PuzzlePiece piezaSeleccionada = null;
    private AudioSource audioSource;
    private GridLayoutGroup gridLayout;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gridLayout = GetComponentInChildren<GridLayoutGroup>();

        for (int i = 0; i < piezas.Count; i++)
        {
            piezas[i].manager = this;
            piezas[i].indiceCorecto = i;
        }

        if (panelRompecabezas != null)
            panelRompecabezas.SetActive(false);

        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    void Start()
    {
        if (panelRompecabezas != null)
            panelRompecabezas.SetActive(false);

        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    public void AbrirRompecabezas()
    {
        piezaSeleccionada = null;

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        if (panelCelular != null)
            panelCelular.SetActive(false);

        if (panelRompecabezas != null)
            panelRompecabezas.SetActive(true);

        StartCoroutine(IniciarRompecabezas());
    }

    IEnumerator IniciarRompecabezas()
    {
        if (gridLayout != null)
            gridLayout.enabled = true;

        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < piezas.Count; i++)
            piezas[i].posicionCorrecta = piezas[i].GetComponent<RectTransform>().anchoredPosition;

        if (gridLayout != null)
            gridLayout.enabled = false;

        MezclarPiezas();
    }

    public void CerrarRompecabezas()
    {
        if (panelRompecabezas != null)
            panelRompecabezas.SetActive(false);

        piezaSeleccionada = null;

        if (gridLayout != null)
            gridLayout.enabled = true;
    }

    void MezclarPiezas()
    {
        // 1. Guarda las posiciones correctas del grid
        Vector2[] posiciones = new Vector2[piezas.Count];
        for (int i = 0; i < piezas.Count; i++)
            posiciones[i] = piezas[i].GetComponent<RectTransform>().anchoredPosition;

        // 2. Fisher-Yates shuffle sobre las posiciones directamente
        for (int i = posiciones.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2 temp = posiciones[i];
            posiciones[i] = posiciones[j];
            posiciones[j] = temp;
        }

        // 3. Evitar que quede igual al original
        bool igualAlOriginal = true;
        for (int i = 0; i < posiciones.Length; i++)
        {
            if (Vector2.Distance(posiciones[i], piezas[i].posicionCorrecta) > 0.5f)
            {
                igualAlOriginal = false;
                break;
            }
        }

        if (igualAlOriginal && piezas.Count >= 2)
        {
            Vector2 temp = posiciones[0];
            posiciones[0] = posiciones[1];
            posiciones[1] = temp;
        }

        // 4. Asignar posiciones mezcladas a cada pieza
        for (int i = 0; i < piezas.Count; i++)
            piezas[i].GetComponent<RectTransform>().anchoredPosition = posiciones[i];
    }

    public void SeleccionarPieza(PuzzlePiece pieza)
    {
        if (piezaSeleccionada == null)
        {
            piezaSeleccionada = pieza;
            pieza.Resaltar(true);
            ReproducirSonido(sonidoClick);
        }
        else if (piezaSeleccionada == pieza)
        {
            pieza.Resaltar(false);
            piezaSeleccionada = null;
        }
        else
        {
            ReproducirSonido(sonidoClick);
            IntercambiarPiezas(piezaSeleccionada, pieza);
            piezaSeleccionada.Resaltar(false);
            piezaSeleccionada = null;
            VerificarVictoria();
        }
    }

    void IntercambiarPiezas(PuzzlePiece a, PuzzlePiece b)
    {
        RectTransform rtA = a.GetComponent<RectTransform>();
        RectTransform rtB = b.GetComponent<RectTransform>();

        Vector2 posTemp = rtA.anchoredPosition;
        rtA.anchoredPosition = rtB.anchoredPosition;
        rtB.anchoredPosition = posTemp;

        Canvas.ForceUpdateCanvases();
    }

    void VerificarVictoria()
    {
        for (int i = 0; i < piezas.Count; i++)
        {
            RectTransform rt = piezas[i].GetComponent<RectTransform>();
            if (Vector2.Distance(rt.anchoredPosition, piezas[i].posicionCorrecta) > 0.5f)
                return;
        }

        Debug.Log("¡Rompecabezas completado!");
        ReproducirSonido(sonidoCompletado);

        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}