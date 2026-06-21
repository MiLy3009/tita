using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Radio : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [Header("Audio")]
    public AudioSource audioLimpio;
    public AudioSource audioEstatica;

    [Header("LEDs")]
    public Image ledUnico;

    [Header("Waveform")]
    public LineRenderer lineRenderer;

    [Header("Cuadros de texto (en orden)")]
    public GameObject cuadro1;
    public GameObject cuadro2;
    public GameObject cuadro3;
    public GameObject cuadro4;

    [Header("Boton Siguiente")]
    public Button botonSiguiente;

    [Header("Cambio de escena")]
    public string nombreEscena = "Cap 3";
    public AudioClip sonidoFinal;

    [Header("Animacion de aplastado")]
    public float escalaAplastado = 0.9f;
    public float duracionAplastado = 0.1f;

    private float[] _spectrum = new float[64];
    private System.Random _rand = new System.Random();

    private bool resuelto = false;
    private int cuadroActual = 0;
    private GameObject[] cuadros;
    private AudioSource audioFinal;
    private Vector3 _escalaOriginal;

    void Start()
    {
        // Crear AudioSource extra para el sonido final
        audioFinal = gameObject.AddComponent<AudioSource>();
        audioFinal.playOnAwake = false;
        audioFinal.volume = 1f;

        lineRenderer.positionCount = 64;
        audioLimpio.volume = 0f;
        audioLimpio.loop = false;
        audioEstatica.volume = 1f;
        audioEstatica.loop = true;
        if (!audioEstatica.isPlaying)
            audioEstatica.Play();

        ledUnico.color = Color.red;

        cuadros = new GameObject[] { cuadro1, cuadro2, cuadro3, cuadro4 };
        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(false);

        if (botonSiguiente != null)
            botonSiguiente.onClick.AddListener(OnBotonSiguienteClick);

        _escalaOriginal = transform.localScale;
    }

    void Update()
    {
        if (resuelto && !audioLimpio.isPlaying)
        {
            if (lineRenderer.gameObject.activeSelf)
                lineRenderer.gameObject.SetActive(false);
            return;
        }

        if (resuelto)
        {
            // Señal limpia: dibuja el espectro real del audio
            audioLimpio.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);
            for (int i = 0; i < 64; i++)
            {
                float x = -3f + i * 0.1f;
                float y = Mathf.Sqrt(_spectrum[i]) * 3f;
                lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            }
            Color limpio = new Color(0.2f, 1f, 0f);
            lineRenderer.startColor = limpio;
            lineRenderer.endColor = limpio;
        }
        else
        {
            // Ruido/estática mientras no se ha presionado el botón
            for (int i = 0; i < 64; i++)
            {
                float x = -3f + i * 0.1f;
                float y = (float)(_rand.NextDouble() * 2.0 - 1.0);
                lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            }
            Color estatica = new Color(1f, 0.4f, 0f);
            lineRenderer.startColor = estatica;
            lineRenderer.endColor = estatica;
        }
    }

    // Animación de "aplastado" al presionar
    public void OnPointerDown(PointerEventData e)
    {
        StopCoroutine(nameof(AnimarAplastado));
        StartCoroutine(AnimarAplastado());
    }

    // Click válido (presionar y soltar dentro del objeto) -> resuelve el puzzle
    public void OnPointerClick(PointerEventData e)
    {
        if (!resuelto)
            Resolver();
    }

    IEnumerator AnimarAplastado()
    {
        transform.localScale = _escalaOriginal * escalaAplastado;
        yield return new WaitForSeconds(duracionAplastado);
        transform.localScale = _escalaOriginal;
    }

    void Resolver()
    {
        resuelto = true;
        audioEstatica.Stop();
        audioLimpio.volume = 1f;
        audioLimpio.Play();
        ledUnico.color = Color.green;
        StartCoroutine(EsperarAudioYMostrar());
    }

    IEnumerator EsperarAudioYMostrar()
    {
        yield return new WaitWhile(() => audioLimpio.isPlaying);
        cuadroActual = 0;
        MostrarCuadro(0);
    }

    void MostrarCuadro(int indice)
    {
        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

        if (indice < cuadros.Length && cuadros[indice] != null)
            cuadros[indice].SetActive(true);

        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(true);
    }

    void OnBotonSiguienteClick()
    {
        if (botonSiguiente != null)
            botonSiguiente.interactable = false;

        cuadroActual++;

        if (cuadroActual < cuadros.Length)
        {
            MostrarCuadro(cuadroActual);
            if (botonSiguiente != null)
                botonSiguiente.interactable = true;
        }
        else
        {
            StartCoroutine(SonidoYCambiarEscena());
        }
    }

    IEnumerator SonidoYCambiarEscena()
    {
        if (sonidoFinal != null && audioFinal != null)
        {
            audioFinal.clip = sonidoFinal;
            audioFinal.Play();
            yield return new WaitWhile(() => audioFinal.isPlaying);
        }
        else
        {
            yield return null;
        }

        SceneManager.LoadScene(nombreEscena);
    }
}