using UnityEngine;
using UnityEngine.UI;

public class SecuenciaBotones : MonoBehaviour
{
    [Header("Botones en orden correcto")]
    public Button[] ordenCorrecto;

    private int indice = 0;
    private bool modoLibre = false;

    void OnEnable()
    {
        if (!modoLibre)
        {
            indice = 0;
            ActualizarBotones();
        }
        else
        {
            foreach (var b in ordenCorrecto)
                b.interactable = true;
        }
    }

    public void IntentarBoton(int posicion)
    {
        if (modoLibre) return;

        if (posicion == indice)
        {
            indice++;

            if (indice >= ordenCorrecto.Length)
            {
                modoLibre = true;
                foreach (var b in ordenCorrecto)
                    b.interactable = true;
            }
            else
            {
                ActualizarBotones();
            }
        }
    }

    void ActualizarBotones()
    {
        for (int i = 0; i < ordenCorrecto.Length; i++)
            ordenCorrecto[i].interactable = (i == indice);
    }
}