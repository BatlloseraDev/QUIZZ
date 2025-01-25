using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Quiz : MonoBehaviour
{

    [Header("Question")]
    [SerializeField]TextMeshProUGUI questionText; 
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>(); 
    QuestionSO currentQuestion;
    
    [Header("Answers"), Tooltip("las respuestas")]
    [SerializeField] GameObject[] answerButtons; 
    int correctAnswerIndex;
    bool hasAnsweredEarly = true; 
    
    [Header("Button Colors")]
    [SerializeField]Sprite defaultAnswerSprite; 
    [SerializeField]Sprite correctAnswerSprite; 
    
    [Header("Timer")]
    [SerializeField] Image timerImgage; 
    Timer timer; 

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText; 
    ScoreKeeper scoreKeeper; 

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar; 
    public bool isComplete; 

    void Awake()
    {
        timer = FindObjectOfType<Timer>(); 
        scoreKeeper = FindObjectOfType<ScoreKeeper>(); 
        progressBar.maxValue = questions.Count; 
        progressBar.value = 0; 

    }

    void Update() 
    {
        timerImgage.fillAmount= timer.fillFraction; 
        if(timer.loadNextQuestion)
        {
            if(progressBar.value == progressBar.maxValue)
            {
                isComplete=true; 
                return; 
            }
            hasAnsweredEarly= false; 
            GetNextQuestion(); 
            timer.loadNextQuestion = false; 
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }

    }


    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly= true; 
        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " +scoreKeeper.CalculateScore() + "%"; 

       
    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;
       
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            questionText.text = "¡Correcto!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers(); 
        }
        else
        {
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Lo siento, la respuesta correcta es:\n" + correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
           
        }
    }

    void GetNextQuestion()
    {

        if(questions.Count>0)
        {
            SetButtonState(true); 
            SetDefaultButtonSPrite();
            GetRandomQuestion();
            DisplayQuestion(); 
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
        
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if(questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);  
        }
       

    }
    void SetDefaultButtonSPrite()
    {
       for(int i = 0; i< answerButtons.Length; i++)
       {
            Image buttonImage = answerButtons[i].GetComponent<Image>(); 
            buttonImage.sprite = defaultAnswerSprite; 
       }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion(); 

        for(int i= 0; i<answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>(); 
            buttonText.text = currentQuestion.GetAnswer(i); 
        }
    }

    void SetButtonState(bool state)
    {
        for(int i= 0; i<answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>(); 
            button.interactable = state; 
        }
    }

}
