using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public GameObject explosionPrefab;
    public LayerMask levelMask;

    public LayerMask levelMaskBlocks;

    private bool exploded = false;


    //public GameObject playerPrefab;

    public GameObject[] players;

    public GameObject[] enemies;

    public int numExplosions = 1;

    private float dist;

    void Awake() {
        players = GameObject.FindGameObjectsWithTag("Player");
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void Start ()
    {
        Invoke("Explode", 3f);
    }

    void FixedUpdate() {
        dist = Vector3.Distance(players[0].gameObject.transform.position, gameObject.transform.position);
        //Debug.Log(dist);

        if(dist <= 0.5f)
        {
            Physics.IgnoreLayerCollision(9, 10, true);
        }
        else
        {
            Physics.IgnoreLayerCollision(9, 10, false);
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

    private IEnumerator CreateExplosions(Vector3 direction) 
    {
        RaycastHit hit;

        RaycastHit hitBlocks;

        Physics.Raycast(transform.position, direction, out hit, numExplosions, levelMask);

        Physics.Raycast(transform.position, direction, out hitBlocks, numExplosions, levelMaskBlocks);

        Debug.DrawLine(transform.position, hit.point, Color.green);
        for (int i = 1; i <= numExplosions; i++) 
        { 
            if(!hit.collider)
            {
                if(hitBlocks.collider)
                {
                    Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
                    break;
                }
                else
                {
                    Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
                }
                
            }
            else if(hit.distance > i) 
            { 
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation); 
            } 
            else 
            {
                break; 
            }
        }
        yield return new WaitForSeconds(0); 
    }
}
