using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public QuestionData[] questions;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;
    public GameObject siguientePanel;
    public GameObject panelPregunta;

    private int currentIndex = 0;

    void Start() => LoadQuestion();

    void LoadQuestion()
    {
        feedbackText.text = "";
        QuestionData q = questions[currentIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
            answerButtons[i].interactable = true;
            answerButtons[i].GetComponent<Image>().color = Color.white;
        }
    }

    void CheckAnswer(int selected)
    {
        bool correct = selected == questions[currentIndex].correctAnswerIndex;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
            Image img = answerButtons[i].GetComponent<Image>();

            if (i == selected && correct)
                img.color = new Color(0.2f, 0.8f, 0.2f);
            else if (i == selected && !correct)
                img.color = new Color(0.9f, 0.2f, 0.2f);
        }

        if (correct)
        {
            feedbackText.text = "¡CORRECTO!";
            feedbackText.color = Color.green;
            Invoke(nameof(IrSiguientePanel), 1.5f);
        }
        else
        {
            feedbackText.text = "INCORRECTO, intenta de nuevo";
            feedbackText.color = Color.red;
            Invoke(nameof(Reintentar), 1.5f);
        }
    }

    void Reintentar() => LoadQuestion();

    void IrSiguientePanel()
    {
        siguientePanel.SetActive(true);
        panelPregunta.SetActive(false);
    }
}