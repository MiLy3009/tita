using UnityEngine;
using UnityEngine.UI;

public class AudioSync : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider slider;
    public AudioClip audioDistorsionado;  // el de reversa
    public AudioClip audioLimpio;         // el original claro

    [Range(0f, 1f)]
    public float valorCorrecto = 0.6f;
    public float tolerancia = 0.05f;

    private bool resuelto = false;

    void Start()
    {
        audioSource.clip = audioDistorsionado;
        audioSource.loop = true;
        audioSource.pitch = 1f;
        audioSource.volume = 1f;
        audioSource.Play();
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
        }
    }
}