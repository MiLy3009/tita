using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SecuenciaBotonesDos : MonoBehaviour
{
    [Header("Botones en orden")]
    [SerializeField] private Button boton1;
    [SerializeField] private Button boton2;

    [Header("Se dispara cuando se completan los 2 pasos")]
    public UnityEvent onSecuenciaCompleta;

    private int pasoActual = 1;

    private void Start()
    {
        boton1.onClick.AddListener(() => PresionarBoton(1));
        boton2.onClick.AddListener(() => PresionarBoton(2));

        ActualizarEstadoBotones();
    }

    private void PresionarBoton(int numeroBoton)
    {
        if (numeroBoton != pasoActual) return;

        pasoActual++;

        if (pasoActual > 2)
        {
            onSecuenciaCompleta?.Invoke();
        }

        ActualizarEstadoBotones();
    }

    private void ActualizarEstadoBotones()
    {
        boton1.interactable = (pasoActual == 1);
        boton2.interactable = (pasoActual == 2);
    }
}