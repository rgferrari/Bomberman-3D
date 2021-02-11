using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    new void Start() {
        base.Start();
    }

    private void FixedUpdate() 
    {
        UpdatePlayerMovement();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBomb();
        }
    }

    private void UpdatePlayerMovement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Up movement
            myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x, myRigidbody.velocity.y, speed * numSpeeds);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Left movement
            myRigidbody.velocity = new Vector3 (-speed * numSpeeds, myRigidbody.velocity.y, myRigidbody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //Down movement
            myRigidbody.velocity = new Vector3 (myRigidbody.velocity.x, myRigidbody.velocity.y, -speed * numSpeeds);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);

        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Right movement
            myRigidbody.velocity = new Vector3 (speed * numSpeeds, myRigidbody.velocity.y, myRigidbody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
        }
    }
}