using System.Collections;
using UnityEngine;
using TMPro;

public class TextoEscribiendose : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoUI;
    [TextArea(3, 10)]
    public string textoCompleto = "Escribe aquí tu texto...";
    public float velocidad = 0.05f; // segundos entre cada letra (se recalcula si usás audio)
    public bool iniciarAutomatico = true;

    [Header("Audio")]
    public AudioSource audioSource;      // Arrastrá acá el AudioSource
    public AudioClip clipVoz;            // El audio que se va a escuchar mientras se escribe
    public bool sincronizarConAudio = true;   // Si está en true, ajusta la velocidad para que el texto termine junto con el audio
    public bool sonidoPorLetra = false;       // Si querés un "tick" por cada letra (tipo máquina de escribir)
    public AudioClip clipTick;                // Sonido corto del tick (opcional)

    private Coroutine coroutinaActual;

    void Start()
    {
        if (iniciarAutomatico)
            MostrarTexto(textoCompleto);
    }

    public void MostrarTexto(string texto)
    {
        if (coroutinaActual != null)
            StopCoroutine(coroutinaActual);

        textoCompleto = texto;
        coroutinaActual = StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        textoUI.text = "";

        float velocidadFinal = velocidad;

        // Si hay audio y queremos sincronizar, calculamos la velocidad según la duración del clip
        if (sincronizarConAudio && clipVoz != null && textoCompleto.Length > 0)
        {
            velocidadFinal = clipVoz.length / textoCompleto.Length;
        }

        // Reproducimos el audio de voz al arrancar a escribir
        if (audioSource != null && clipVoz != null)
        {
            audioSource.clip = clipVoz;
            audioSource.Play();
        }

        foreach (char letra in textoCompleto)
        {
            textoUI.text += letra;

            // Sonido tipo "tick" por cada letra (opcional, independiente del audio de voz)
            if (sonidoPorLetra && audioSource != null && clipTick != null && letra != ' ')
            {
                audioSource.PlayOneShot(clipTick);
            }

            yield return new WaitForSeconds(velocidadFinal);
        }

        coroutinaActual = null;
    }

    public void Saltar()
    {
        if (coroutinaActual != null)
        {
            StopCoroutine(coroutinaActual);
            textoUI.text = textoCompleto;
            coroutinaActual = null;

            // Si saltamos, también cortamos el audio de voz
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    public bool EstaEscribiendo()
    {
        return coroutinaActual != null;
    }
}