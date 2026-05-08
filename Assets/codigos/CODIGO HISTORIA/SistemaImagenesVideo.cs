using UnityEngine;
using UnityEngine.Video;

public class SistemaImagenesVideo : MonoBehaviour
{
    [Header("Cuadro 1 - encima del video 1")]
    public GameObject cuadro1;

    [Header("Cuadros 2-6 - encima del video 2 (en orden)")]
    public GameObject[] cuadros2al6;

    [Header("Botón siguiente")]
    public GameObject botonSiguiente;

    [Header("Raspadita")]
    public ScratchTransition scratchTransition;

    [Header("Panel final - después del cuadro 6")]
    public GameObject panelFinal;

    private int indiceActual = 0;
    private bool enFase2 = false;

    void Start()
    {
        // Solo mostrar cuadro 1
        cuadro1.SetActive(true);
        botonSiguiente.SetActive(true);

        // Ocultar cuadros 2-6
        foreach (GameObject c in cuadros2al6)
            c.SetActive(false);

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

        // Fase 2: avanzar cuadros 2-6
        cuadros2al6[indiceActual].SetActive(false);
        indiceActual++;

        if (indiceActual < cuadros2al6.Length)
        {
            cuadros2al6[indiceActual].SetActive(true);
            botonSiguiente.SetActive(true);
        }
        else
        {
            // Terminaron todos → panel final
            botonSiguiente.SetActive(false);
            if (panelFinal != null)
                panelFinal.SetActive(true);
        }
    }

    // Llamado automáticamente cuando termina el video 2
    public void ActivarCuadrosDespuesVideo2()
    {
        enFase2 = true;
        indiceActual = 0;
        cuadros2al6[0].SetActive(true);
        botonSiguiente.SetActive(true);
    }
}