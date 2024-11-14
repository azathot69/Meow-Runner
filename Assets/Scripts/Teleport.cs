using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    #region Variables
    [Header("Go To Scene")]
    public int goToScene;

    #endregion

    //Go to next scene when colliding with object

    private void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            GoToScene();
            Debug.Log("Collided with player");
        }
    }


    /// <summary>
    /// Go to Selected scene
    /// </summary>
    void GoToScene()
    {
        SceneManager.LoadScene(goToScene);
        Debug.Log("Going to scene");
    }
}
