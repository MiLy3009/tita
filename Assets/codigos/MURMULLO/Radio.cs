using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Radio : MonoBehaviour, IDragHandler, IBeginDragHandler
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

    [Header("Cambio de panel")]
    public GameObject panelActual;
    public GameObject panelSiguiente;

    [Header("Sonido al terminar")]
    public AudioClip sonidoFinal;

    private float _tuning = 0f;
    private Vector2 _lastMouse;
    private float _angulo = -135f;
    private float[] _spectrum = new float[64];
    private float _noise = 1f;
    private System.Random _rand = new System.Random();

    private bool resuelto = false;
    private int cuadroActual = 0;
    private GameObject[] cuadros;

    void Start()
    {
        lineRenderer.positionCount = 64;
        audioLimpio.volume = 0f;
        audioLimpio.loop = false;
        audioEstatica.volume = 1f;
        ActualizarLED();

        cuadros = new GameObject[] { cuadro1, cuadro2, cuadro3, cuadro4 };
        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(false);

        if (botonSiguiente != null)
            botonSiguiente.onClick.AddListener(OnBotonSiguienteClick);
    }

    void Update()
    {
        // Si ya termino el audio limpio apaga la onda
        if (resuelto && !audioLimpio.isPlaying)
        {
            if (lineRenderer.gameObject.activeSelf)
                lineRenderer.gameObject.SetActive(false);
            return;
        }

        audioLimpio.GetSpectrumData(_spectrum, 0, FFTWindow.Hamming);
        _noise = Mathf.Lerp(_noise, 1f - _tuning, Time.deltaTime * 5f);

        for (int i = 0; i < 64; i++)
        {
            float x = -3f + i * 0.1f;
            float cleanY = Mathf.Sqrt(_spectrum[i]) * 3f;
            float noiseY = (float)(_rand.NextDouble() * 2.0 - 1.0);
            float y = Mathf.Lerp(cleanY, noiseY, _noise);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }

        Color c = Color.Lerp(new Color(1f, 0.4f, 0f), new Color(0.2f, 1f, 0f), _tuning);
        lineRenderer.startColor = c;
        lineRenderer.endColor = c;
    }

    public void OnBeginDrag(PointerEventData e) => _lastMouse = e.position;

    public void OnDrag(PointerEventData e)
    {
        if (resuelto) return;

        float delta = (e.position.y - _lastMouse.y) * 0.5f;
        _angulo = Mathf.Clamp(_angulo + delta, -135f, 135f);
        _lastMouse = e.position;
        transform.rotation = Quaternion.Euler(0, 0, -_angulo);
        _tuning = Mathf.InverseLerp(-135f, 135f, _angulo);
        AplicarTuning();
    }

    void AplicarTuning()
    {
        audioEstatica.volume = Mathf.Clamp01(1f - (_tuning / 0.85f));
        audioLimpio.volume = 0f;
        ActualizarLED();

        if (_tuning >= 0.95f && !resuelto)
        {
            resuelto = true;
            audioEstatica.Stop();
            audioLimpio.volume = 1f;
            audioLimpio.Play();
            StartCoroutine(EsperarAudioYMostrar());
        }
    }

    void ActualizarLED()
    {
        ledUnico.color = Color.Lerp(Color.red, Color.green, _tuning);
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
            StartCoroutine(SonidoYCambiarPanel());
        }
    }

    IEnumerator SonidoYCambiarPanel()
    {
        if (sonidoFinal != null)
            audioLimpio.PlayOneShot(sonidoFinal);

        yield return new WaitForSeconds(sonidoFinal != null ? sonidoFinal.length : 0f);

        if (panelActual != null) panelActual.SetActive(false);
        if (panelSiguiente != null) panelSiguiente.SetActive(true);
    }
}