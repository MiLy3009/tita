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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

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
        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        MezclarPiezas();
    }

    public void CerrarRompecabezas()
    {
        if (panelRompecabezas != null)
            panelRompecabezas.SetActive(false);

        piezaSeleccionada = null;
    }

    void MezclarPiezas()
    {
        // Crea lista de índices y la mezcla
        List<int> indices = new List<int>();
        for (int i = 0; i < piezas.Count; i++)
            indices.Add(i);

        // Fisher-Yates shuffle
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[j];
            indices[j] = temp;
        }

        // Evita que quede igual al original
        bool igualAlOriginal = true;
        for (int i = 0; i < indices.Count; i++)
        {
            if (indices[i] != i)
            {
                igualAlOriginal = false;
                break;
            }
        }

        if (igualAlOriginal && piezas.Count >= 2)
        {
            int temp = indices[0];
            indices[0] = indices[1];
            indices[1] = temp;
        }

        // Reordena los hijos en la jerarquía según los índices mezclados
        // El Grid Layout Group usa el sibling index para posicionar
        for (int i = 0; i < piezas.Count; i++)
        {
            piezas[indices[i]].transform.SetSiblingIndex(i);
        }

        Canvas.ForceUpdateCanvases();
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
        // Intercambia el sibling index entre las dos piezas
        int indexA = a.transform.GetSiblingIndex();
        int indexB = b.transform.GetSiblingIndex();

        a.transform.SetSiblingIndex(indexB);
        b.transform.SetSiblingIndex(indexA);

        Canvas.ForceUpdateCanvases();
    }

    void VerificarVictoria()
    {
        // La victoria es cuando cada pieza está en su sibling index correcto
        for (int i = 0; i < piezas.Count; i++)
        {
            if (piezas[i].transform.GetSiblingIndex() != i)
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