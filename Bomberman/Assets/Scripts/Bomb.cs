using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    void Start ()
    {
        Invoke("Explode", 2f); //Call Explode in 3 seconds
    }

    void Explode ()
    {
        Destroy(gameObject, 0.3f); //Destroy the actual bomb in 0.3 seconds, after all coroutines have finished
    }
}
