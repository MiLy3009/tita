using UnityEngine;

public class RutaSelector : MonoBehaviour
{
    public void SeleccionarRutaB()
    {
        GameState.esRutaB = true;
    }

    public void SeleccionarRutaA()
    {
        GameState.esRutaB = false;
    }
}