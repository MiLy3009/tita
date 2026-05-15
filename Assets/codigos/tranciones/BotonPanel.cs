using UnityEngine;

public class BotonPanel : MonoBehaviour
{
    public GameObject panelActual;
    public GameObject panelNuevo;
    public bool abrirRompecabezas = false;
    private PanelTransitionManager manager;
    private PuzzleManager puzzleManager;

    void Start()
    {
        manager = FindAnyObjectByType<PanelTransitionManager>();
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
    }

    public void AlAplastar()
    {
        if (abrirRompecabezas && puzzleManager != null)
        {
            panelActual.SetActive(false);
            puzzleManager.AbrirRompecabezas();
        }
        else
        {
            manager.CambiarPanel(panelActual, panelNuevo);
        }
    }
}
