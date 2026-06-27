using System.Collections;
using UnityEngine;
using TMPro;

public class TextoEscribiendose : MonoBehaviour
{
    [Header("Configuración")]
    public TextMeshProUGUI textoUI;
    [TextArea(3, 10)]
    public string textoCompleto = "Escribe aquí tu texto...";
    public float velocidad = 0.05f; // segundos entre cada letra
    public bool iniciarAutomatico = true;

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

        foreach (char letra in textoCompleto)
        {
            textoUI.text += letra;
            yield return new WaitForSeconds(velocidad);
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
        }
    }

    public bool EstaEscribiendo()
    {
        return coroutinaActual != null;
    }
}