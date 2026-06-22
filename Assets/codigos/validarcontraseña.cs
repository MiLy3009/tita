using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ValidarContrasenia : MonoBehaviour
{
    HashSet<string> contraseniasCorrectas;
    string contraseniaUsuario;

    [Header("Configuración de UI")]
    public TMP_InputField ingresoUsuario;
    public TextMeshProUGUI textoMsj;
    public GameObject cartelitoMsj;

    [Header("Paneles de Navegación")]
    public GameObject panelActual;
    public GameObject panelSiguiente;

    [Header("Sonidos")]
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    private AudioSource fuente;

    void Start()
    {
        fuente = Camera.main.gameObject.AddComponent<AudioSource>();
        fuente.playOnAwake = false;

        contraseniasCorrectas = new HashSet<string>
        {
            "04042026",
            "04.04.2026",
            "04_04_2026",
            "04-04-2026",
            "04/04/2026",
            "04 04 2026",
            "040426"
        };

        if (cartelitoMsj != null) cartelitoMsj.SetActive(false);
        if (panelSiguiente != null) panelSiguiente.SetActive(false);
    }

    public void validarContrasenia()
    {
        contraseniaUsuario = ingresoUsuario.text;

        if (contraseniasCorrectas.Contains(contraseniaUsuario))
        {
            if (sonidoCorrecto != null) fuente.PlayOneShot(sonidoCorrecto);

            cartelitoMsj.SetActive(true);
            textoMsj.text = "Bienvenida Emma";

            Invoke("CambiarDePanel", 1.2f);
        }
        else
        {
            if (sonidoIncorrecto != null) fuente.PlayOneShot(sonidoIncorrecto);

            cartelitoMsj.SetActive(true);
            textoMsj.text = "Contraseña Incorrecta";
            StartCoroutine(OcultarMensaje(1.5f));
        }
    }

    IEnumerator OcultarMensaje(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (cartelitoMsj != null) cartelitoMsj.SetActive(false);
        ingresoUsuario.text = "";
        ingresoUsuario.ActivateInputField();
    }

    void CambiarDePanel()
    {
        if (panelActual != null && panelSiguiente != null)
        {
            panelActual.SetActive(false);
            if (cartelitoMsj != null) cartelitoMsj.SetActive(false);
            panelSiguiente.SetActive(true);
        }
    }
}