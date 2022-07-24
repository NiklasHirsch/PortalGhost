using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transform Container", menuName
= "ScriptableObjects/Game State")]
public class GameState : ScriptableObject, ISerializationCallbackReceiver
{
    public bool initMenuOpen = true;
    public bool initGamePaused = true;
    public bool initGameStarted = false;

    [NonSerialized]
	public bool menuOpen;
    [NonSerialized]
    public bool gamePaused;
    [NonSerialized]
    public bool gameStarted;

    public void OnAfterDeserialize()
    {
        menuOpen = initMenuOpen;
        gamePaused = initGamePaused;
        gameStarted = initGameStarted;
    }

    public void ToggleMenu(GameObject gui)
    {
		menuOpen = !menuOpen;
		gui.SetActive(menuOpen);
        TogglePause();
	}

	public void PauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
    }

	public void UnpauseGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
    }

	public void TogglePause()
    {
		if (gamePaused)
        {
            UnpauseGame();
        } else
        {
            PauseGame();
        }
    }

    public void OnBeforeSerialize(){}
}