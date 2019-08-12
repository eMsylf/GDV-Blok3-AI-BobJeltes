using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour {

    public GameState gameState; 
    public static LevelManager levelManager;

    int currentSceneIndex;

    public Text WinText;
    public Text LoseText;
    public Text ButtonInstructions;


    public float RestartQuitDelay = 90f;
    private float restartQuitDelay;

    void Awake() {
        levelManager = this;
    }

    private void Start() {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        restartQuitDelay = RestartQuitDelay;
    }

    public void WinGame() {
        gameState = GameState.Win;
    }

    public void LoseGame() {
        gameState = GameState.Lose;
    }

    private void LoadPreviousLevel() {
        Debug.Log("Main menu");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void RestartLevel() {
        Debug.Log("Restart level");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel() {
        Debug.Log("Next level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    void Update() {
        switch (gameState) {
            case (GameState.Active): {
                    if (currentSceneIndex == 0) {
                        if (Input.GetButtonDown("Fire2")) {
                            LoadNextLevel();
                        } else if (Input.GetButtonDown("Fire1")) {
                            QuitGame();
                        }
                    }
                    break;
                }
            case (GameState.Win): {
                    WinText.enabled = true;
                    ShowButtonInstructions();
                    break;
                }
            case (GameState.Lose): {
                    LoseText.enabled = true;
                    ShowButtonInstructions();
                    break;
                }
        }
    }

    private void ShowButtonInstructions() {
        if (restartQuitDelay <= 0) {
            ButtonInstructions.enabled = true;
            if (Input.GetButtonDown("Fire2")) {
                RestartLevel();
            } else if (Input.GetButton("Fire1")) {
                LoadPreviousLevel();
            } 
        } else {
            restartQuitDelay--;
        }
    }

    public enum GameState {
        Active,
        Win,
        Lose
    }


}
