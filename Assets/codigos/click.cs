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
    }

    public void Reproducir()
    {
        fuente.PlayOneShot(clip);
    }
}
