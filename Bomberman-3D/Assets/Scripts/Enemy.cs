using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject astarGrid;
    List<Node> path;
    Vector3 target;
    float bombRadius;
    float step = 1f;
    bool bombExists;
    bool isInDangerZone;
    bool powerUpExists;
    float myDistanceToPowerUp, playerDistanceToPowerUp;
    float myDistanceToPlayer;
    int currentState;

    void Start(){
        Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        if(bombExists && isInDangerZone){
            // Wait 1s
            // Go to the nearest safe zone
        }

        else if(powerUpExists){
            // Check distance between player and power up
            if(myDistanceToPowerUp <= playerDistanceToPowerUp){
                // Move towards the power up
            } 
        }

        else if(myDistanceToPlayer < bombRadius){
            // Move towards the nearest blowing up zone
            // Place a bomb
        }

        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer(){
        path = astarGrid.GetComponent<AstarGrid>().FinalPath;
        if(path != null && path.Count > 0){
            target = path[0].vPosition;
            target = new Vector3(target.x, transform.position.y, target.z);
            LookAtMovementDirection(path[0].vPosition);
            transform.position = Vector3.MoveTowards(transform.position, target, step * Time.deltaTime);
            if(transform.position == target)
                path.RemoveAt(0);
        }
    }

    void LookAtMovementDirection(Vector3 dir){
        Vector3 lookDir = dir - transform.position;
        lookDir.y = 0f; //this is the critical part, this makes the look direction perpendicular to 'up'
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        //transform.position += transform.forward * 1.5f * Time.deltaTime;
    }
}
