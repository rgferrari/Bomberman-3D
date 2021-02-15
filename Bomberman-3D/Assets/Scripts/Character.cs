using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float speed;
    public int playerNumber = 1;
    public int numBombs = 1;
    public int numExplosions = 1;
    protected Rigidbody myRigidbody;
    protected Transform myTransform;
    public GameObject bombPrefab;
    public bool canDropBombs = true;
    public bool canMove = true;
    public bool dead = false;
    public Text textNumBomb;
    public Text textNumExplosion;
    public Text textNumSpeed;

    [HideInInspector]
    public int numKeys;

    protected virtual void Start() { 
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        myTransform = transform;

        textNumBomb.text = (numBombs).ToString();
        textNumExplosion.text = (numExplosions).ToString();
        textNumSpeed.text = (speed).ToString();
    }

    protected void DropBomb() 
    {
        bool col = false;
        if (bombPrefab) 
        {
            GameObject[] objs;
            objs = GameObject.FindGameObjectsWithTag("Bomb");
            
            Vector3 player = new Vector3(Mathf.RoundToInt(transform.position.x), 
                            transform.position.y, Mathf.RoundToInt(transform.position.z));

            int numBombsInField = 0;
            foreach(GameObject obj in objs) {
                if(obj.GetComponent<Bomb>().playerNumber == playerNumber)
                    numBombsInField++;

                if(Vector3.Distance(player, obj.transform.position) < 1){
                    col = true;
                }
            }

            //Debug.Log(objs.Length);

            if (col == false){
                if(numBombsInField < numBombs)
                {
                    GameObject bomb = Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(transform.position.x), 
                            transform.position.y, Mathf.RoundToInt(transform.position.z)),
                            bombPrefab.transform.rotation);
                    
                    bomb.GetComponent<Bomb>().numExplosions = numExplosions;
                    bomb.GetComponent<Bomb>().playerNumber = playerNumber;
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
            if(playerNumber == 1)
                textNumBomb.text = numBombs.ToString();
            //Debug.Log("Other Collider:" + other.name);
        }

        if (other.CompareTag ("numExplosions"))
        {
            numExplosions += 1;
            if(playerNumber == 1)
                textNumExplosion.text = numExplosions.ToString();
            //Debug.Log("Other Collider:" + other.name);
        }

        if (other.CompareTag ("numSpeeds"))
        {
            if(playerNumber == 2 && speed < 6.5)
                speed += 1;
            
            if(playerNumber == 1 && speed < 10)
                speed += 1;
            //Debug.Log ("Speed = " + numSpeeds);
            if(playerNumber == 1){
                textNumSpeed.text = (speed).ToString();
            }
            //Debug.Log("Other Collider:" + other.name);
            //Destroy(gameObject);
        }
    }
}
