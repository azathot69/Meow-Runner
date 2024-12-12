using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

	public GameObject pauseMenuUI;

	public void OnLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void OnLevel2()
    {
        SceneManager.LoadScene(2);
    }
    public void OnLevel3()
    {
        SceneManager.LoadScene(3);
    }

    public void OnQuitButton ()
    {
        Application.Quit();
    }
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			
            if (GameIsPaused == true)
			{
				Resume();
            }
            else
			{
				Pause();
            }
			
        }


    }

	// Will resume the game where you last were
	public void Resume ()
	{
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
        Cursor.visible = false;

    }

    // Will Pause the whole game
    public void Pause ()
	{
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
        Cursor.visible = true;
    }

    // Loading the Main Menu
    public void LoadMenu ()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}

}
