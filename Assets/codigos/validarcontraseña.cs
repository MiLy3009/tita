using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ValidarContrasenia : MonoBehaviour
{
    string contraseniaCorrecta;
    string contraseniaUsuario;

    [Header("Configuración de UI")]
    public TMP_InputField ingresoUsuario;
    public TextMeshProUGUI textoMsj;
    public GameObject cartelitoMsj;

    [Header("Paneles de Navegación")]
    public GameObject panelActual;    // Arrastra aquí: Panel clave
    public GameObject panelSiguiente; // Arrastra aquí: Panel desbloqueada

    void Start()
    {
        contraseniaCorrecta = "12345";

        if (cartelitoMsj != null) cartelitoMsj.SetActive(false);
        if (panelSiguiente != null) panelSiguiente.SetActive(false);
    }

    public void validarContrasenia()
    {
        contraseniaUsuario = ingresoUsuario.text;

        if (contraseniaUsuario == contraseniaCorrecta)
        {
            // 1. Mostramos mensaje de éxito brevemente
            cartelitoMsj.SetActive(true);
            textoMsj.text = "Bienvenido";

            // 2. Ejecutamos el cambio total
            Invoke("CambiarDePanel", 1.2f); // 1.2 segundos para que sea rápido
        }
        else
        {
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
            // APAGAMOS TODO LO ANTERIOR
            panelActual.SetActive(false);    // Apaga el teclado y el fondo de clave
            if (cartelitoMsj != null) cartelitoMsj.SetActive(false); // Apaga el "Bienvenido"

            // ENCENDEMOS EL NUEVO
            panelSiguiente.SetActive(true); // Enciende la pantalla azul de "Última conexión"
        }
    }
}