using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public GameObject explosionPrefab;
    public LayerMask levelMask;
    private bool exploded = false;

    void Start ()
    {
        Invoke("Explode", 3f); //Call Explode in 3 seconds
    }

    void Update()
    {
        /*Vector3 forward = transform.TransformDirection(Vector3.forward) + new Vector3(0,0,0.5f);
        Vector3 right = transform.TransformDirection(Vector3.right) + new Vector3(0.5f,0,0);
        Vector3 back = transform.TransformDirection(Vector3.back) + new Vector3(0,0,-0.5f);
        Vector3 left = transform.TransformDirection(Vector3.left) + new Vector3(-0.5f,0,0);
        Debug.DrawRay(transform.position, forward, Color.green);
        Debug.DrawRay(transform.position, right, Color.green);
        Debug.DrawRay(transform.position, back, Color.green);
        Debug.DrawRay(transform.position, left, Color.green);*/
    }

    void Explode ()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        // StartCoroutine(CreateExplosions(Vector3.forward + new Vector3(0,0,0.5f)));
        // StartCoroutine(CreateExplosions(Vector3.right + new Vector3(0.5f,0,0)));
        // StartCoroutine(CreateExplosions(Vector3.back + new Vector3(0,0,-0.5f)));
        // StartCoroutine(CreateExplosions(Vector3.left + new Vector3(-0.5f,0,0)));

        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        GetComponent<SphereCollider>().gameObject.SetActive(false);
        Destroy(gameObject, .3f);
    }

    public void OnTriggerEnter(Collider other) 
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            Debug.Log(other);
            CancelInvoke("Explode");
            Explode();
        }  

    }

    private IEnumerator CreateExplosions(Vector3 direction) 
    {
        RaycastHit hit;

        int dist = 2;

        Physics.Raycast(transform.position, direction, out hit, 2, levelMask);
        Debug.DrawLine(transform.position, hit.point, Color.green);
        for (int i = 1; i <= dist; i++) 
        { 
            if(!hit.collider)
            {
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
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
        yield return new WaitForSeconds(.2f); 
    }
}
