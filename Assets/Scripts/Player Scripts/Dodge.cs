using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    [Header("Dodge")]
    [SerializeField]
    private bool canDodge;

    public float dodgeSpeed;

    public KeyCode dodgeAction = KeyCode.Mouse1;

    public float dodgeCooldown;

    private float countDown;
    public float countDownMax;
    public bool stratCountdown;

    [Header("References")]
    private PlayerScript pm;
    private Rigidbody rb;
    public Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerScript>();
        countDown = countDownMax;
    }

    // Update is called once per frame
    void Update()
    {
        DodgeInput();
    }

    private void FixedUpdate()
    {
        if (stratCountdown)
        {
            //Timer
            if (countDown > 0) countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                canDodge = false;

                countDown = countDownMax;

                stratCountdown = false;

                //Reset Variables
                Invoke(nameof(DodgeReset), dodgeCooldown);
            }
        }
    }

    /// <summary>
    /// Get Dodge Input
    /// </summary>
    private void DodgeInput()
    {
        if (Input.GetKeyDown(dodgeAction) && canDodge)
        {
            stratCountdown = true;
            PlayerDodge();
        }
    }

    private void PlayerDodge()
    {
        //Get Player Invincible Variable
        pm.invincible = true;
        rb.useGravity = false;

        

        //Move Player
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(rb.velocity * dodgeSpeed, ForceMode.Impulse);

        

        
    }

    private void DodgeReset()
    {
        pm.invincible = false;
        rb.useGravity = true;
        canDodge = true;
    }
}
