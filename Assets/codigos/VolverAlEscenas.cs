using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAlEscenas : MonoBehaviour
{
    public GameObject panelAjustes;

 
    public void CargarCap2()
    {
        SceneManager.LoadScene("Cap 2");
    }

    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void AbrirAjustes()
    {
        panelAjustes.SetActive(true);
    }

    public void CerrarAjustes()
    {
        panelAjustes.SetActive(false);
    }

    public void CargarAjustes()
    {
        PlayerPrefs.SetString("escenaAnterior", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("AbrirAjustes", 1);
        SceneManager.LoadScene("menu");
    }
}