using UnityEngine;
using UnityEngine.SceneManagement;

public class RegresarEscena : MonoBehaviour
{
    private static string escenaAnterior = "Menu"; // por defecto el menú

    // Llama esto ANTES de ir a Ajustes
    public static void GuardarEscenaActual()
    {
        escenaAnterior = SceneManager.GetActiveScene().name;
    }

    // Ponlo en el botón Regresar
    public void Regresar()
    {
        SceneManager.LoadScene(escenaAnterior);
    }
}