using UnityEngine;
using UnityEngine.UI;

public class SonidoClick : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private AudioSource fuente;

    private void Awake()
    {
        fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;

        // precarga el audio para evitar retardo al primer click
        fuente.clip = clip;
        fuente.volume = 0f;
        fuente.Play();
        fuente.Stop();
        fuente.volume = 1f;
    }

    public void Reproducir()
    {
        fuente.PlayOneShot(clip);
    }
}

