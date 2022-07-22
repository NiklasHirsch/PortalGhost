using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GUIController : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    Button buttonResume;
    Button buttonNewGame;
    Button buttonSettings;
    Button buttonQuit;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        buttonResume = root.Q<Button>("ButtonResume");
        buttonNewGame = root.Q<Button>("ButtonNewGame");
        buttonSettings = root.Q<Button>("ButtonSettings");
        buttonQuit = root.Q<Button>("ButtonQuit");

        if (!gameState.gameStarted)
        {
            buttonNewGame.style.display = DisplayStyle.None;
            buttonResume.text = "Start Game";
        } else
        {
            //OnResumeGame();
        }

        Debug.Log("gui enabled");
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        buttonResume.clicked += OnResumeGame;
        buttonSettings.clicked += OnSettings;
        buttonQuit.clicked += OnQuit;
        buttonNewGame.clicked += OnNewGame;
    }

    //The lockState has to be checked in Update() for now because on Game Startup the lockState somehow always switches to CursorLockMode.Locked
    void Update()
    {
        if (UnityEngine.Cursor.lockState != CursorLockMode.None)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }

    private void OnDisable()
    {
        Debug.Log("gui disabled");
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

        //buttonResume.clicked -= OnResumeGame;
        //buttonNewGame.clicked -= OnNewGame;
        //buttonSettings.clicked -= OnSettings;
        //buttonQuit.clicked -= OnQuit;
    }

    private void OnNewGame()
    {
        gameState.gameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnResumeGame()
    {
        gameState.gameStarted = true;
        gameState.ToggleMenu(gameObject);
    }

    private void OnSettings()
    {

    }

    private void OnQuit()
    {
        Application.Quit();
    }

}
