using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    Rigidbody Rig;

    //Prefabs
    public GameObject bombPrefab;

    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    void Start() {
        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody> ();
        myTransform = transform;
    }

    private void FixedUpdate() 
    {
        Vector3 Position = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        rigidBody.velocity = Position * speed;
    }

    void Update ()
    {
        DropBomb();
    }

    private void DropBomb() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bombPrefab) 
            {
                Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x), 
                            myTransform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                            bombPrefab.transform.rotation);  

            }
        }
    }
    
}