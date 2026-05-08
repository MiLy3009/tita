using UnityEngine;

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
            cuadro1.SetActive(false);
            botonSiguiente.SetActive(false);
            scratchTransition.ActivarRaspadita();
            return;
        }

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

    public void ActivarCuadrosDespuesVideo2()
    {
        enFase2 = true;
        indiceActual = 0;

        if (panelVideo2 != null)
            panelVideo2.SetActive(true);

        cuadros2al6[0].SetActive(true);
        botonSiguiente.SetActive(true);
    }
}