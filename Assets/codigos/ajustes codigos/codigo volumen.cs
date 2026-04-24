using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicaVolumen : MonoBehaviour
{
    public Slider slider;
    public float sliderValue;

    [Header("Imagen de Volumen")]
    public Image imagenVolumen;      // Un solo componente Image

    [Header("Sprites")]
    public Sprite spriteSinSonido;   // Con la X
    public Sprite spriteSonidoMedio; // Una ondita
    public Sprite spriteSonidoAlto;  // Dos onditas

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        sliderValue = slider.value;
        AudioListener.volume = slider.value;
        ActualizarIcono();
    }

    public void ChangeSlider(float valor)
    {
        sliderValue = valor;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);
        AudioListener.volume = sliderValue;
        ActualizarIcono();
    }

    public void ActualizarIcono()
    {
        if (sliderValue == 0)
        {
            imagenVolumen.sprite = spriteSinSonido;   // X
        }
        else if (sliderValue <= 0.5f)
        {
            imagenVolumen.sprite = spriteSonidoMedio; // Una ondita
        }
        else
        {
            imagenVolumen.sprite = spriteSonidoAlto;  // Dos onditas
        }
    }
}