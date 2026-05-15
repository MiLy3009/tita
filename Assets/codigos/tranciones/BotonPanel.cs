using UnityEngine;

public class BotonPanel : MonoBehaviour
{
    public GameObject panelActual;
    public GameObject panelNuevo;
    private PanelTransitionManager manager;

    void Start()
    {
        manager = FindAnyObjectByType<PanelTransitionManager>();
    }

    public void AlAplastar()
    {
        manager.CambiarPanel(panelActual, panelNuevo);
    }
}