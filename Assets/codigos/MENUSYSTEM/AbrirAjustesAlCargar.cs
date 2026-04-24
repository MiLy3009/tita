using UnityEngine;

public class AbrirAjustesAlCargar : MonoBehaviour
{
    public GameObject panelAjustes;

    void Start()
    {
        if (PlayerPrefs.GetInt("AbrirAjustes") == 1)
        {
            panelAjustes.SetActive(true);
            PlayerPrefs.SetInt("AbrirAjustes", 0); // lo resetea
        }
    }
}