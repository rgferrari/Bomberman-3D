using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public GameObject player;
    public GameObject astarGrid;
    //List<Node> path;
    Vector3 target;
    bool bombExists;
    bool powerUpExists;
    float myDistanceToPowerUp, playerDistanceToPowerUp;
    int currentState;
    Vector3 targetCratePos;
    float myDistanceToCrate = int.MaxValue;

    LayerMask layerMask;

    new void Start(){
        layerMask = LayerMask.GetMask("Wall", "Crate");
        // 0 for chasing the player
        // 1 for chasing blocks
        currentState = 1; //Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> powerUpPositions = astarGrid.GetComponent<AstarGrid>().powerUpPositions;
        
        //Vector3 nearestPowerUp = astarGrid.GetComponent<AstarGrid>().nearestPowerUp;
        
        List<Vector3> cratePositions = astarGrid.GetComponent<AstarGrid>().cratePositions;
        
        float myDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        bool foundPathToPowerUp = false;

        Vector3 closestPowerUp = Vector3.zero;

        float minDistanceToPowerUp = int.MaxValue;

        foreach(Vector3 powerUpPosition in powerUpPositions){
            float distanceToPowerUp = Vector3.Distance(transform.position, powerUpPosition);
            if(distanceToPowerUp < minDistanceToPowerUp){
                if(HasPath(powerUpPosition)){
                    foundPathToPowerUp = true;
                    Debug.Log("Has Path to " + powerUpPosition);
                    minDistanceToPowerUp = distanceToPowerUp;
                    closestPowerUp = powerUpPosition;
                }
            }
        }

        // if(Input.GetKey (KeyCode.A))
        //     DropBomb();
        
        if(isInDangerZone()){
            Vector3 nearestSafePos = astarGrid.GetComponent<AstarGrid>().nearestSafeNode.vPosition;
            MoveTowards(nearestSafePos);
        }

        else if(foundPathToPowerUp){
            // myDistanceToPowerUp = Vector3.Distance(transform.position, closestPowerUp);
            // playerDistanceToPowerUp = Vector3.Distance(player.transform.position, closestPowerUp);
            // if(myDistanceToPowerUp <= playerDistanceToPowerUp){
            //     MoveTowards(closestPowerUp);
            // } 
            if(foundPathToPowerUp)
                MoveTowards(closestPowerUp);
        }

        // If gets near player
        else if(myDistanceToPlayer < numExplosions && currentState == 0){
            DropBomb();
            currentState = Random.Range(0,2);
        }
        // If gets near crate
        else if(myDistanceToCrate <= 1.5f && currentState == 1){
            DropBomb();
            myDistanceToCrate = int.MaxValue;
            currentState = 1; //Random.Range(0,2);
        }

        // If has to chase player
        else if(currentState == 0)
            MoveTowards(player.transform.position);
        
        // If has to chase crate
        else if(cratePositions.Count > 0){
            if(currentState == 1){
                targetCratePos = cratePositions[0];
                Vector3[] targetCratePosNeighbors = {Vector3.forward, Vector3.back, Vector3.right, Vector3.left};
                foreach(Vector3 neighbor in targetCratePosNeighbors){
                    Vector3 pos = targetCratePos + neighbor;
                    if (!Physics.CheckBox(pos, new Vector3(0.1f,0.1f,0.1f),new Quaternion(0,0,0,0), layerMask))
                    {
                        if(HasPath(pos)){
                            myDistanceToCrate = Vector3.Distance(transform.position, targetCratePos);
                            MoveTowards(pos);
                            break;
                        }
                    }
                }
            }
        }

    }

    void MoveTowards(Vector3 targetPos, int discountPath = 0){
        // astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
        // astarGrid.GetComponent<Pathfinding>().TargetPosition = targetPos;
        astarGrid.GetComponent<Pathfinding>().FindPath(transform.position, targetPos);
        List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;

        // if(path.Count > 1){
        //     for(int i = 0; i < discountPath; i++){
        //         path.RemoveAt(path.Count - 1);
        //     }
        // }

        if(path.Count > 0){
            target = path[0].vPosition;
            target = new Vector3(target.x, transform.position.y, target.z);
            LookAtMovementDirection(path[0].vPosition);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if(transform.position == target)
                path.RemoveAt(0);
        }
    }

    bool HasPath(Vector3 targetPos){
        astarGrid.GetComponent<Pathfinding>().FindPath(transform.position, targetPos);
        List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;
        return path.Count > 0 ? true : false;
    }

    bool isInDangerZone(){
        foreach(Vector3 explosionPosition in astarGrid.GetComponent<AstarGrid>().explosionPositions){
            if (Vector3.Distance(transform.position, explosionPosition) <= .5f)
                return true;
        }
        return false;
    }

    void LookAtMovementDirection(Vector3 dir){
        Vector3 lookDir = dir - transform.position;
        lookDir.y = 0f; //this is the critical part, this makes the look direction perpendicular to 'up'
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    }
}
