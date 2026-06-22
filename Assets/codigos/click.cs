using UnityEngine;
using UnityEngine.UI;

public class SonidoClick : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private AudioSource fuente;

    private void Start()
    {
        fuente = gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;

        GetComponent<Button>().onClick.AddListener(ReproducirSonido);
    }

    private void ReproducirSonido()
    {
        fuente.PlayOneShot(clip);
    }
}