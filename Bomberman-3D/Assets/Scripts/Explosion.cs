using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Explode ()
    {
        Destroy(gameObject, .1f);
    }
}
