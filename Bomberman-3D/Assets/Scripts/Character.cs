﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float speed;
    public int playerNumber = 1;
    public int numBombs;
    public float numSpeeds;
    public int numExplosions;
    protected Rigidbody myRigidbody;
    protected Transform myTransform;
    public GameObject bombPrefab;
    public bool canDropBombs = true;
    public bool canMove = true;
    public bool dead = false;
    public Text textNumBomb;
    public Text textNumExplosion;
    public Text textNumSpeed;

    protected virtual void Start() { 
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        myTransform = transform;
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

            foreach(GameObject obj in objs) {
                if(Vector3.Distance(player, obj.transform.position) < 1){
                    col = true;
                }
            }

            //Debug.Log(objs.Length);

            if (col == false){
                if(objs.Length < numBombs)
                {
                    bombPrefab.gameObject.GetComponent<Bomb>().numExplosions = numExplosions;
                    Instantiate(bombPrefab, new Vector3(Mathf.RoundToInt(transform.position.x), 
                            transform.position.y + 0.5f, Mathf.RoundToInt(transform.position.z)),
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

        if (other.CompareTag ("numBombs"))
        {
            numBombs += 1;
            textNumBomb.text = numBombs.ToString();
            //Debug.Log("Other Collider:" + other.name);
        }

        if (other.CompareTag ("numExplosions"))
        {
            numExplosions += 1;
            textNumExplosion.text = numExplosions.ToString();
            //Debug.Log("Other Collider:" + other.name);
        }

        if (other.CompareTag ("numSpeeds"))
        {
            numSpeeds += 0.2f;
            //Debug.Log ("Speed = " + numSpeeds);
            textNumSpeed.text = (speed * numSpeeds).ToString();
            //Debug.Log("Other Collider:" + other.name);
            //Destroy(gameObject);
        }
    }
}