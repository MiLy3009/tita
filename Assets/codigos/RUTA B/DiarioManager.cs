using UnityEngine;

public class DiarioManager : MonoBehaviour
{
    public GameObject panelPregunta;
    public GameObject panelSiguiente;

    public void AlAbrirDiario()
    {
        if (GameState.esRutaB)
            panelPregunta.SetActive(true);
        else
            panelSiguiente.SetActive(true);
    }
}