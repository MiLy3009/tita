using UnityEngine;

public class BotonPanel : MonoBehaviour
{
    public GameObject panelActual;
    public GameObject panelNuevo;

    public void AlAplastar()
    {
        panelActual.SetActive(false);
        panelNuevo.SetActive(true);
    }
}