using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonDash : MonoBehaviour
{
    ThirdPersonMovement moveScript;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 2;
    private float nextDashTime = 0;

    
    
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextDashTime)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(Dash());
                nextDashTime = Time.time + dashCooldown;
            }
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            transform.Translate(Vector3.forward * dashSpeed);
            //moveScript.controller.Move(moveScript.moveDir * dashSpeed * Time.deltaTime);
            yield return null;
        }

    }
}
