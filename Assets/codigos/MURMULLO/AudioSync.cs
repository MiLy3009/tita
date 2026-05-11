using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioSync : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;
    public AudioClip audioDistorsionado;
    public AudioClip audioLimpio;

    [Range(0f, 1f)]
    public float valorCorrecto = 0.6f;
    public float tolerancia = 0.05f;

    [Header("Cuadros de texto (en orden)")]
    public GameObject cuadro1;
    public GameObject cuadro2;
    public GameObject cuadro3;
    public GameObject cuadro4;

    [Header("Botón Siguiente")]
    public Button botonSiguiente;

    [Header("Cambio de panel (solo al 4to cuadro)")]
    public GameObject panelActual;
    public GameObject panelSiguiente;

    private bool resuelto = false;
    private int cuadroActual = 0;
    private GameObject[] cuadros;

    void Start()
    {
        audioSource.clip = audioDistorsionado;
        audioSource.loop = true;
        audioSource.Play();

        cuadros = new GameObject[] { cuadro1, cuadro2, cuadro3, cuadro4 };

        // Ocultar todos los cuadros al inicio
        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

        // Ocultar botón al inicio
        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(false);
    }

    public void OnSliderCambiado()
    {
        if (resuelto) return;

        float distancia = Mathf.Abs(slider.value - valorCorrecto);

        if (distancia < tolerancia)
        {
            resuelto = true;
            audioSource.clip = audioLimpio;
            audioSource.loop = false;
            audioSource.Play();

            StartCoroutine(EsperarAudioYMostrar());
        }
    }

    IEnumerator EsperarAudioYMostrar()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        cuadroActual = 0;
        MostrarCuadro(0);
    }

    void MostrarCuadro(int indice)
    {
        // Ocultar todos
        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

        // Mostrar solo el actual
        if (indice < cuadros.Length && cuadros[indice] != null)
            cuadros[indice].SetActive(true);

        // Mostrar botón
        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(true);
    }

    // Este método lo llamas desde el OnClick del botón en el Inspector
    public void OnBotonSiguienteClick()
    {
        cuadroActual++;

        if (cuadroActual < cuadros.Length)
        {
            MostrarCuadro(cuadroActual);
        }
        else
        {
            // Pasaron los 4 cuadros → cambiar panel
            if (panelActual != null) panelActual.SetActive(false);
            if (panelSiguiente != null) panelSiguiente.SetActive(true);
        }
    }
}