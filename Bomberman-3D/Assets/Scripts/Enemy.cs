using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public GameObject player;
    public GameObject astarGrid;
    List<Node> path;
    Vector3 target;
    bool bombExists;
    bool isInDangerZone;
    bool powerUpExists;
    float myDistanceToPowerUp, playerDistanceToPowerUp;
    float myDistanceToPlayer;
    int currentState;

    new void Start(){
        // 0 for chasing the player
        // 1 for chasing blocks
        currentState = Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        myDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(Input.GetKey (KeyCode.A))
            DropBomb();
        
        isInDangerZone = false;
        foreach(Vector3 explosionPosition in astarGrid.GetComponent<AstarGrid>().explosionPositions){
            if (Vector3.Distance(transform.position, explosionPosition) <= 2f)
                isInDangerZone = true;
        }

        if(isInDangerZone){
            Vector3 nearestSafePos = astarGrid.GetComponent<AstarGrid>().nearestSafeNode.vPosition;
            MoveTowards(nearestSafePos);
        }

        else if(powerUpExists){
            // Check distance between player and power up
            if(myDistanceToPowerUp <= playerDistanceToPowerUp){
                // Move towards the power up
            } 
        }

        // else if(myDistanceToPlayer < numExplosions){
        //     // Move towards the nearest blowing up zone
        //     // Place a bomb
        //     currentState = Random.Range(0,2);
        // }

        //else if(currentState == 0){
            // Wait 1s
        else
            MoveTowards(player.transform.position);
        //}
        
        //else if(currentState == 1)
            // Wait 1s
            //MoveTowardsCrate();
    }

    void MoveTowards(Vector3 targetPos){
        astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
        astarGrid.GetComponent<Pathfinding>().TargetPosition = targetPos;
        path = astarGrid.GetComponent<AstarGrid>().FinalPath;
        if(path != null && path.Count > 0){
            target = path[0].vPosition;
            target = new Vector3(target.x, transform.position.y, target.z);
            LookAtMovementDirection(path[0].vPosition);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if(transform.position == target)
                path.RemoveAt(0);
        }
    }

    void LookAtMovementDirection(Vector3 dir){
        Vector3 lookDir = dir - transform.position;
        lookDir.y = 0f; //this is the critical part, this makes the look direction perpendicular to 'up'
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
