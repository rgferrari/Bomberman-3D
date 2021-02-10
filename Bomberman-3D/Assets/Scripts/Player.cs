using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public int playerNumber = 1;

    Rigidbody Rig;

    //Prefabs
    public GameObject bombPrefab;

    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

    public bool canDropBombs = true;
    //Can the player drop bombs?
    public bool canMove = true;
    //Can the player move?
    public bool dead = false;
    //Is this player dead?
    
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
        bool col = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bombPrefab) 
            {
                GameObject[] objs;
                objs = GameObject.FindGameObjectsWithTag("Bomb");
                Vector3 player = new Vector3(Mathf.RoundToInt(myTransform.position.x), 
                                myTransform.position.y, Mathf.RoundToInt(myTransform.position.z));

                foreach(GameObject obj in objs) {
                    if(Vector3.Distance(player, obj.transform.position) < 1){
                        col = true;
                    }
                }

                Debug.Log(objs.Length);

                if (col == false){
                    Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x), 
                                myTransform.position.y, Mathf.RoundToInt(myTransform.position.z)),
                                bombPrefab.transform.rotation);
                }
            }
        }
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Explosion"))
        {
            Debug.Log ("P" + playerNumber + " hit by explosion!");
            //Destroy(gameObject);
        }
    }
    
}