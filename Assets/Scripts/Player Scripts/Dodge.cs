using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [Header("Dodge")]
    [SerializeField]
    private bool canDodge;

    public float dodgeCoolDownTimer;
    public float dodgeCoolDown;
    public KeyCode dodgeAction = KeyCode.Mouse1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DodgeInput();
    }

    /// <summary>
    /// Get Dodge Input
    /// </summary>
    private void DodgeInput()
    {
        if (Input.GetKey(dodgeAction) && canDodge)
        {
            canDodge = false;

            PlayerDodge();

            Invoke(nameof(DodgeReset), dodgeCoolDown);
        }
    }

    private void PlayerDodge()
    {
        //Get Player Invincible Variable

        //Get player input

        //Move Player
    }

    private void DodgeReset()
    {
        canDodge = true;
    }
}
