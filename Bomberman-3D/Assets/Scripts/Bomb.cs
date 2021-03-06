﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public GameObject explosionPrefab;
    public LayerMask levelMask;

    public LayerMask levelMaskBlocks;

    private bool exploded = false;

    [HideInInspector]
    public GameObject[] players, enemies;
    private GameObject character;

    public int numExplosions = 1;

    private float dist;

    public int playerNumber;

    public List<Vector3> explosionPositions;

    void Start ()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Debug.Log(enemies.Length);

        foreach(GameObject p in players){
            if(playerNumber == p.GetComponent<Character>().playerNumber){
                character = p;
                break;
            }
        }
        foreach(GameObject e in enemies){
            if(playerNumber == e.GetComponent<Character>().playerNumber){
                character = e;
                break;
            }
        }

        CalculateExplosions(Vector3.forward);
        CalculateExplosions(Vector3.back);
        CalculateExplosions(Vector3.right);
        CalculateExplosions(Vector3.left);

        Invoke("Explode", 3f);
    }

    void FixedUpdate() {
        dist = Vector3.Distance(character.transform.position, gameObject.transform.position);
        if(dist <= 0.5f)
        {
            if(playerNumber == 1)
                Physics.IgnoreLayerCollision(9, 10, true);
            else{
                
                Physics.IgnoreLayerCollision(12, 10, true);}
        }
        else
        {
            if(playerNumber == 1)
                Physics.IgnoreLayerCollision(9, 10, false);
            else{
                
                Physics.IgnoreLayerCollision(12, 10, false);}
        }
    }

    void Explode ()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        GetComponent<BoxCollider>().gameObject.SetActive(false);
        Destroy(gameObject, .3f);
    }

    public void OnTriggerEnter(Collider other) 
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }  

    }

    private void CalculateExplosions(Vector3 direction){
        RaycastHit hitWall;

        RaycastHit hitBlocks;

        Physics.Raycast(transform.position, direction, out hitWall, numExplosions, levelMask);

        Physics.Raycast(transform.position, direction, out hitBlocks, numExplosions, levelMaskBlocks);

        Debug.DrawLine(transform.position, hitWall.point, Color.green);
        for (int i = 1; i <= numExplosions; i++) 
        { 
            if(!hitWall.collider || hitWall.distance > i )
            {
                if(hitBlocks.collider)
                {
                    explosionPositions.Add(transform.position + (i * direction));
                    if(Vector3.Distance(hitBlocks.transform.position, transform.position + (i * direction)) < 1f ){
                        break;
                    }
                }
                else
                {
                    explosionPositions.Add(transform.position + (i * direction));
                }
            }
            else 
            {
                break; 
            }
        }
    }

    // Must calculate again because of eventual grid updates
    private IEnumerator CreateExplosions(Vector3 direction) 
    {
        RaycastHit hitWall;

        RaycastHit hitBlocks;

        Physics.Raycast(transform.position, direction, out hitWall, numExplosions, levelMask);

        Physics.Raycast(transform.position, direction, out hitBlocks, numExplosions, levelMaskBlocks);
        
        for (int i = 1; i <= numExplosions; i++) 
        { 
            if(!hitWall.collider || hitWall.distance > i )
            {
                if(hitBlocks.collider)
                {
                    Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
                    if(Vector3.Distance(hitBlocks.transform.position, transform.position + (i * direction)) < 0.5f ){
                        break;
                    }
                }
                else
                {
                    Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
                }
            }
            else 
            {
                break; 
            }
        }
        yield return new WaitForSeconds(0); 
    }
}