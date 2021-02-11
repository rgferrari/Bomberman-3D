using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public int playerNumber = 1;

    public int numBombs = 1;

    public float numSpeeds = 1.1f;

    public int numExplosions = 1;

    Rigidbody Rig;

    //bool colide = false;

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
        //Vector3 Position = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        
        //rigidBody.velocity = Position * speed;

        UpdatePlayerMovement();
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

                //Debug.Log(objs.Length);

                if (col == false){
                    if(objs.Length < numBombs)
                    {
                        Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(myTransform.position.x), 
                                myTransform.position.y + 0.5f, Mathf.RoundToInt(myTransform.position.z)),
                                bombPrefab.transform.rotation);
                    }
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

        if (other.CompareTag ("numBombs"))
        {
            numBombs += 1;
            Debug.Log("Other Collider:" + other.name);
        }

        if (other.CompareTag ("numExplosions"))
        {
            numExplosions += 1;
            Debug.Log("Other Collider:" + other.name);
        }

        if (other.CompareTag ("numSpeeds"))
        {
            numSpeeds += 0.2f;
            //Debug.Log ("Speed = " + numSpeeds);
            Debug.Log("Other Collider:" + other.name);
            //Destroy(gameObject);
        }
    }

    private void UpdatePlayerMovement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Up movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, speed * numSpeeds);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //Left movement
            rigidBody.velocity = new Vector3 (-speed * numSpeeds, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //Down movement
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -speed * numSpeeds);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);

        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //Right movement
            rigidBody.velocity = new Vector3 (speed * numSpeeds, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
        }
    }
    
}