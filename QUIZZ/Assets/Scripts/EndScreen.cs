using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndScreen : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreKeeper ScoreKeeper; 

    void Awake()
    {
        ScoreKeeper = FindObjectOfType<ScoreKeeper>();
    }


    public void ShowFinalScore()
    {
        finalScoreText.text = "¡Felicidades Sinji!\nTu has tenido una puntuacion de "+ 
                                ScoreKeeper.CalculateScore()+ "%";

    }
 
}
