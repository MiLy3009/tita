using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public PuzzleManager manager;
    [HideInInspector] public int indiceCorecto;
    [HideInInspector] public Vector2 posicionCorrecta;
    

    public Image imagen;
    private Color colorOriginal;

    void Awake()
    {
        //imagen = GetComponent<Image>();
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

        if (activar)
            imagen.color = new Color(0.7f, 1f, 0.7f, 1f); // verde claro al seleccionar
        else
            imagen.color = colorOriginal;
    }
}