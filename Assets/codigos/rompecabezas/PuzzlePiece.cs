using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int indiceCorecto;
    [HideInInspector] public int indiceActual;
    [HideInInspector] public Vector2 posicionCorrecta;
    [HideInInspector] public PuzzleManager manager;

    private Image imagen;
    private Color colorOriginal;

    [Header("Color de selección")]
    public Color colorSeleccionado = new Color(1f, 0.8f, 0f, 1f);

    void Awake()
    {
        imagen = GetComponent<Image>();
        if (imagen != null)
            colorOriginal = imagen.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager != null)
            manager.SeleccionarPieza(this);
    }

    public void Resaltar(bool activar)
    {
        if (imagen == null) return;
        imagen.color = activar ? colorSeleccionado : colorOriginal;
    }
}