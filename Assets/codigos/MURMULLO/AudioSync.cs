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

    [Header("Sonido al terminar")]
    public AudioClip sonidoFinal;

    private bool resuelto = false;
    private int cuadroActual = 0;
    private GameObject[] cuadros;

    void Start()
    {
        audioSource.clip = audioDistorsionado;
        audioSource.loop = true;
        audioSource.Play();

        cuadros = new GameObject[] { cuadro1, cuadro2, cuadro3, cuadro4 };

        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

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
        // Espera a que el audio limpio termine
        yield return new WaitWhile(() => audioSource.isPlaying);

        cuadroActual = 0;
        MostrarCuadro(0);
    }

    void MostrarCuadro(int indice)
    {
        // Ocultar todos los cuadros
        foreach (GameObject c in cuadros)
            if (c != null) c.SetActive(false);

        // Mostrar solo el actual
        if (indice < cuadros.Length && cuadros[indice] != null)
            cuadros[indice].SetActive(true);

        // Mostrar botón
        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(true);
    }

    public void OnBotonSiguienteClick()
    {
        // Deshabilitar botón para evitar doble clic
        if (botonSiguiente != null)
            botonSiguiente.interactable = false;

        cuadroActual++;

        if (cuadroActual < cuadros.Length)
        {
            MostrarCuadro(cuadroActual);

            // Volver a habilitar el botón
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
        // Suena el audio final
        if (sonidoFinal != null)
            audioSource.PlayOneShot(sonidoFinal);

        // Espera a que termine el sonido
        yield return new WaitForSeconds(sonidoFinal != null ? sonidoFinal.length : 0f);

        // Cambia de panel
        if (panelActual != null) panelActual.SetActive(false);
        if (panelSiguiente != null) panelSiguiente.SetActive(true);
    }
}