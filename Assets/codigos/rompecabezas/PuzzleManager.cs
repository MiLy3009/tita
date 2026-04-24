using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Piezas del rompecabezas (en orden correcto: pieza1..pieza9)")]
    public List<PuzzlePiece> piezas = new List<PuzzlePiece>();

    [Header("Efectos (opcional)")]
    public AudioClip sonidoClick;
    public AudioClip sonidoCompletado;
    public GameObject panelVictoria;

    private PuzzlePiece piezaSeleccionada = null;
    private AudioSource audioSource;
    private Vector2[] posicionesCorrectas; // ← Vector2 para UI

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        posicionesCorrectas = new Vector2[piezas.Count];
        for (int i = 0; i < piezas.Count; i++)
        {
            // anchoredPosition es la posición correcta en UI
            posicionesCorrectas[i] = piezas[i].GetComponent<RectTransform>().anchoredPosition;
            piezas[i].indiceCorecto = i;
            piezas[i].manager = this;
        }

        MezclarPiezas();
    }

    void MezclarPiezas()
    {
        List<Vector2> mezcladas = new List<Vector2>(posicionesCorrectas);

        for (int i = mezcladas.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2 temp = mezcladas[i];
            mezcladas[i] = mezcladas[j];
            mezcladas[j] = temp;
        }

        for (int i = 0; i < piezas.Count; i++)
        {
            piezas[i].GetComponent<RectTransform>().anchoredPosition = mezcladas[i];
        }
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
            IntercambiarPosiciones(piezaSeleccionada, pieza);
            piezaSeleccionada.Resaltar(false);
            piezaSeleccionada = null;
            VerificarVictoria();
        }
    }

    void IntercambiarPosiciones(PuzzlePiece a, PuzzlePiece b)
    {
        RectTransform rtA = a.GetComponent<RectTransform>();
        RectTransform rtB = b.GetComponent<RectTransform>();

        Vector2 posA = rtA.anchoredPosition;
        rtA.anchoredPosition = rtB.anchoredPosition;
        rtB.anchoredPosition = posA;
    }

    void VerificarVictoria()
    {
        for (int i = 0; i < piezas.Count; i++)
        {
            Vector2 pos = piezas[i].GetComponent<RectTransform>().anchoredPosition;
            if (Vector2.Distance(pos, posicionesCorrectas[i]) > 0.5f)
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