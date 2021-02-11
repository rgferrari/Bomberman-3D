using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //GameObject  ChildGameObject;

    void Start()
    {
        //ChildGameObject = transform.GetChild (0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0,50*Time.deltaTime,0); //rotates 50 degrees per second around z axis
    }

    public void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag ("Player"))
        {
            Destroy(gameObject);
        }
    }
}
