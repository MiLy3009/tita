using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SecuenciaBotonesChat : MonoBehaviour
{
    [Header("Botones en orden")]
    [SerializeField] private Button boton1;
    [SerializeField] private Button boton2;
    [SerializeField] private Button boton3;

    [Header("Se dispara cuando se completan los 3 pasos")]
    public UnityEvent onSecuenciaCompleta;

    private int pasoActual = 1; // empieza en el boton 1

    private void Start()
    {
        boton1.onClick.AddListener(() => PresionarBoton(1));
        boton2.onClick.AddListener(() => PresionarBoton(2));
        boton3.onClick.AddListener(() => PresionarBoton(3));

        ActualizarEstadoBotones();
    }

    private void PresionarBoton(int numeroBoton)
    {
        if (numeroBoton != pasoActual) return; // seguridad extra, aunque ya estaria desactivado

        pasoActual++;

        if (pasoActual > 3)
        {
            // se completo la secuencia 1 -> 2 -> 3
            onSecuenciaCompleta?.Invoke();
        }

        ActualizarEstadoBotones();
    }

    private void ActualizarEstadoBotones()
    {
        boton1.interactable = (pasoActual == 1);
        boton2.interactable = (pasoActual == 2);
        boton3.interactable = (pasoActual == 3);
    }
}