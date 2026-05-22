using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void SiguienteNivel()
    {
        int siguiente = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(siguiente);
    }
}