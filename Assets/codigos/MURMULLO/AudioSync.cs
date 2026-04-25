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

    [Header("Botón Siguiente")]
    public Button botonSiguiente; // Asigna en el Inspector

    private bool resuelto = false;

    void Start()
    {
        audioSource.clip = audioDistorsionado;
        audioSource.loop = true;
        audioSource.pitch = 1f;
        audioSource.volume = 1f;
        audioSource.Play();

        // Oculta el botón al inicio
        if (botonSiguiente != null)
            botonSiguiente.gameObject.SetActive(false);
    }

    public void OnSliderCambiado()
    {
        if (resuelto) return;

        float valor = slider.value;
        float distancia = Mathf.Abs(valor - valorCorrecto);

        if (distancia < tolerancia)
        {
            resuelto = true;
            audioSource.clip = audioLimpio;
            audioSource.loop = false;
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
            audioSource.Play();

            Debug.Log("¡Sincronizado!");

            // Muestra el botón Siguiente al sincronizar
            if (botonSiguiente != null)
                botonSiguiente.gameObject.SetActive(true);
        }
    }
}