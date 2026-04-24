using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaMenu : MonoBehaviour
{
    public GameObject panelAjustes;

    void Start()
    {
        // Si venimos del juego, abre ajustes directo
        if (PlayerPrefs.GetInt("AbrirAjustes", 0) == 1)
        {
            PlayerPrefs.SetInt("AbrirAjustes", 0);
            panelAjustes.SetActive(true);
        }
    }

    public void CerrarAjustes()
    {
        string escenaAnterior = PlayerPrefs.GetString("escenaAnterior", "");

        if (escenaAnterior == "menu" || escenaAnterior == "")
        {
            panelAjustes.SetActive(false); // estaba en menú, solo cierra
        }
        else
        {
            SceneManager.LoadScene(escenaAnterior); // regresa al juego
        }
    }
}