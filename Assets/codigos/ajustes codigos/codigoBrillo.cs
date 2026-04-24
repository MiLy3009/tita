using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicaBrillo : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;
    public Image panelBrillo;

    [Header("Icono Brillo")]
    public Image imagenBrillo;

    [Header("Sprites")]
    public Sprite spriteBajo;
    public Sprite spriteMedio;
    public Sprite spriteAlto;

    void Start()
    {
        if (slider != null)
        {
            slider.value = PlayerPrefs.GetFloat("brillo", 0.5f);
            sliderValue = slider.value;

            if (panelBrillo != null)
                ActualizarBrillo(slider.value);

            ActualizarIcono();
        }
        else
        {
            Debug.LogError("Error: No has arrastrado el 'Slider' al script LogicaBrillo en el Inspector.");
        }
    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("brillo", sliderValue);

        if (panelBrillo != null)
            ActualizarBrillo(valor);

        ActualizarIcono();
    }

    void ActualizarBrillo(float alpha)
    {
        panelBrillo.color = new Color(
            panelBrillo.color.r,
            panelBrillo.color.g,
            panelBrillo.color.b,
            alpha
        );
    }

    void ActualizarIcono()
    {
        if (imagenBrillo == null) return;

        if (sliderValue <= 0.33f)
            imagenBrillo.sprite = spriteBajo;
        else if (sliderValue <= 0.66f)
            imagenBrillo.sprite = spriteMedio;
        else
            imagenBrillo.sprite = spriteAlto;
    }
}