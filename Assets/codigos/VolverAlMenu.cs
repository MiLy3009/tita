using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverAlMenu : MonoBehaviour
{
    public GameObject panelAjustes;

    public void CargarMenu()
    {
        SceneManager.LoadScene("menu");
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
        // Guarda que venimos del JUEGO
        PlayerPrefs.SetString("escenaAnterior", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("AbrirAjustes", 1);
        SceneManager.LoadScene("menu");
    }
}