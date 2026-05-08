using UnityEngine;
using UnityEngine.Video;

public class SistemaImagenesVideo : MonoBehaviour
{
    [Header("Cuadro 1 - encima del video 1")]
    public GameObject cuadro1;

    [Header("Panel video 2")]
    public GameObject panelVideo2;

    [Header("Cuadros 2-6 - encima del video 2 (en orden)")]
    public GameObject[] cuadros2al6;

    [Header("Botón siguiente cuadros")]
    public GameObject botonSiguiente;

    [Header("Raspadita")]
    public ScratchTransition scratchTransition;

    [Header("Panel final - después del cuadro 6")]
    public GameObject panelFinal;

    private int indiceActual = 0;
    private bool enFase2 = false;

    void Start()
    {
        // ✅ Cuadro 1 aparece al inicio con el video 1
        cuadro1.SetActive(true);
        botonSiguiente.SetActive(true);

        foreach (GameObject c in cuadros2al6)
            c.SetActive(false);

        if (panelVideo2 != null)
            panelVideo2.SetActive(false);

        if (panelFinal != null)
            panelFinal.SetActive(false);
    }

    public void Siguiente()
    {
        if (!enFase2)
        {
            // Click en cuadro 1 → raspadita
            cuadro1.SetActive(false);
            botonSiguiente.SetActive(false);
            scratchTransition.ActivarRaspadita();
            return;
        }

        // Avanzar cuadros 2-6
        cuadros2al6[indiceActual].SetActive(false);
        indiceActual++;

        if (indiceActual < cuadros2al6.Length)
        {
            cuadros2al6[indiceActual].SetActive(true);
            botonSiguiente.SetActive(true);
        }
        else
        {
            botonSiguiente.SetActive(false);
            if (panelFinal != null)
                panelFinal.SetActive(true);
        }
    }

    // ✅ Se llama cuando EMPIEZA el video 2, no cuando termina
    public void ActivarCuadrosDespuesVideo2()
    {
        enFase2 = true;
        indiceActual = 0;

        if (panelVideo2 != null)
            panelVideo2.SetActive(true);

        // Mostrar primer cuadro inmediatamente
        cuadros2al6[0].SetActive(true);
        botonSiguiente.SetActive(true);
    }
}