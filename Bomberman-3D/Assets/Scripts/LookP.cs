using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookP : MonoBehaviour
{
    public GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var lookDir = players[0].transform.position - transform.position;
        lookDir.y = 800f; //this is the critical part, this makes the look direction perpendicular to 'up'
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        transform.position += transform.forward * 1.5f * Time.deltaTime;

        //transform.position = Vector3.Lerp(transform.position, players[0].transform.position, 0.1f);
    }
}
