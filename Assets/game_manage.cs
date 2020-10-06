using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_manage : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed; 
    public static game_manage Instance;
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public GameObject victoryPage;
    public Text scoreText;
    public Text energyText;
    public enum PageState{
        None,
        Start,
        GameOver,
        Countdown,
        Victory
    }
    public int score = 0;
    public int energy = 404;
    public bool gameOver = false;
    

    public bool GameOver { get{ return gameOver; }}
    void Awake(){
        Instance = this;
    
        
        
    }
    void OnEnable(){
        countDownText.OnCountdownFinished += OnCountdownFinished;
        tap_control.OnPlayerDied += OnPlayerDied;
        tap_control.OnPlayerScored += OnPlayerScored;
        tap_control.OnPlayerTapped += OnPlayerTapped;
    }
    void OnDisable(){
        countDownText.OnCountdownFinished -= OnCountdownFinished;
        tap_control.OnPlayerDied -= OnPlayerDied;
        tap_control.OnPlayerScored -= OnPlayerScored;
        tap_control.OnPlayerTapped -= OnPlayerTapped;
    }
    void OnCountdownFinished(){
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        energy = 404;
    }
    void OnPlayerDied(){
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("highscore");
        if (score > savedScore){
            PlayerPrefs.SetInt("highscore", score);
        }
        SetPageState(PageState.GameOver);
    }
    void OnPlayerTapped(){
        energy--;
        energyText.text = "ENERGY: " + energy.ToString();
    }
    void OnPlayerScored(){

        score++;
        if(score == 404){
            SetPageState(PageState.Victory);
            gameOver = true;
        }
        scoreText.text = score.ToString();

    }
    void SetPageState(PageState state){
        
        switch(state){
            case PageState.None:
            startPage.SetActive(false);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(false);
            victoryPage.SetActive(false);
            gameOver = false;
     
            break;
            case PageState.Start:
            startPage.SetActive(true);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(false);
            victoryPage.SetActive(false);
            gameOver = true;
         
            

            break;
            case PageState.GameOver:
            startPage.SetActive(false);
            gameOverPage.SetActive(true);
            countdownPage.SetActive(false);
            victoryPage.SetActive(false);
            gameOver = true;
          
            break;
            case PageState.Countdown:
            startPage.SetActive(false);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(true);
            victoryPage.SetActive(false);
            gameOver = true;
         
            break;
            case PageState.Victory:
            startPage.SetActive(false);
            gameOverPage.SetActive(false);
            countdownPage.SetActive(false);
            victoryPage.SetActive(true);
            gameOver = true;

            break;
        } 
    }
    public void ConfirmGameOver(){
        OnGameOverConfirmed();
        scoreText.text = "0";
        energyText.text = "404";
        SetPageState(PageState.Start);
        gameOver = true;
    }
    public void StartGame(){
        SetPageState(PageState.Countdown);
        gameOver = true;

    }
     
}
