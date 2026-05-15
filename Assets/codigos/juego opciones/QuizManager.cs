using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public Button[] answerButtons;
    public GameObject siguientePanel;
    public GameObject panelPregunta;
    public int correctAnswerIndex;

    [Header("Botón Continuar")]
    public Button continuarButton;

    void Start()
    {
        if (continuarButton != null)
        {
            continuarButton.gameObject.SetActive(false);
            continuarButton.onClick.AddListener(IrSiguientePanel);
        }

        CargarBotones();
    }

    void CargarBotones()
    {
        // Oculta el botón Continuar
        if (continuarButton != null)
            continuarButton.gameObject.SetActive(false);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => VerificarRespuesta(index));
            answerButtons[i].interactable = true;
            answerButtons[i].GetComponent<Image>().color = Color.white;
        }
    }

    void VerificarRespuesta(int selected)
    {
        bool correcto = selected == correctAnswerIndex;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
            Image img = answerButtons[i].GetComponent<Image>();

            if (i == selected && correcto)
                img.color = new Color(0.2f, 0.8f, 0.2f); // Verde
            else if (i == selected && !correcto)
                img.color = new Color(0.9f, 0.2f, 0.2f); // Rojo
        }

        if (correcto)
        {
            if (continuarButton != null)
                continuarButton.gameObject.SetActive(true);
        }
        else
        {
            Invoke(nameof(Reintentar), 1.5f);
        }
    }

    void Reintentar() => CargarBotones();

    void IrSiguientePanel()
    {
        siguientePanel.SetActive(true);
        panelPregunta.SetActive(false);
    }
}