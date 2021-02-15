using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public GameObject player;
    public GameObject astarGrid;
    //List<Node> path;
    Vector3 target;
    float playerDistanceToPowerUp;
    int currentState;
    Vector3 targetCratePos;
    float myDistanceToCrate = int.MaxValue;

    LayerMask layerMask;

    bool hasTargetCrate;
    List<Vector3> cratePositions;

    bool wasInDanger;

    float timer = 2f;

    bool thinking = true;
    Vector3 boxPosition;

    new void Start(){
        layerMask = LayerMask.GetMask("Wall", "Crate");
        // 0 for chasing the player
        // 1 for chasing blocks
        currentState = 0; //Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        if(thinking){
            Debug.Log("Qual o sentido da vida???");
            timer -= Time.deltaTime;
        }

        if(timer <= 0){
            timer = 3f;
            thinking = false;
        }

        List<Vector3> powerUpPositions = astarGrid.GetComponent<AstarGrid>().powerUpPositions;
        
        //Vector3 nearestPowerUp = astarGrid.GetComponent<AstarGrid>().nearestPowerUp;
        
        cratePositions = astarGrid.GetComponent<AstarGrid>().cratePositions;
        
        float myDistanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        Vector3 targetPowerUp = new Vector3();

        float minDistanceToPowerUp = int.MaxValue;

        foreach(Vector3 powerUp in powerUpPositions){
            if(minDistanceToPowerUp > Vector3.Distance(transform.position, powerUp)){
                minDistanceToPowerUp = Vector3.Distance(transform.position, powerUp);
                targetPowerUp = powerUp;
            }
        }

        float distanceToPowerUp = Vector3.Distance(transform.position, targetPowerUp);
        float playerDistanceToPowerUp = Vector3.Distance(player.transform.position, targetPowerUp);

        // bool foundPathToPowerUp = false;

        // Vector3 closestPowerUp = Vector3.zero;

        // float minDistanceToPowerUp = int.MaxValue;

        // foreach(Vector3 powerUpPosition in powerUpPositions){
        //     float distanceToPowerUp = Vector3.Distance(transform.position, powerUpPosition);
        //     if(distanceToPowerUp < minDistanceToPowerUp){
        //         if(!HasPath(powerUpPosition)){
        //             foundPathToPowerUp = true;
        //             Debug.Log("Has Path to " + powerUpPosition);
        //             minDistanceToPowerUp = distanceToPowerUp;
        //             closestPowerUp = powerUpPosition;
        //         }
        //     }
        // }

        // if(Input.GetKey (KeyCode.A))
        //     DropBomb();

        if(wasInDanger){
            if(!isInDangerZone()){
                thinking = true;
                wasInDanger = false;
            }
        }
        

        if(isInDangerZone()){
            wasInDanger = true;

            Vector3 nearestSafePos = astarGrid.GetComponent<AstarGrid>().nearestSafeNode.vPosition;
            nearestSafePos.y = transform.position.y;
            float distanceToSafeZone = Vector3.Distance(transform.position, nearestSafePos);
        
            Debug.Log("Estou Fugindo");

            //Debug.Log("NearestSafePos" + nearestSafePos);

            //MoveTowards(nearestSafePos);
            nearestSafePos.y = transform.position.y;
            astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
            astarGrid.GetComponent<Pathfinding>().TargetPosition = nearestSafePos;
            List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;
            Vector3 lastPosition = nearestSafePos + transform.forward;
            Node box = new Node(false, false, lastPosition, 1, 2);

            path.Add(box);

            if(path != null && path.Count > 0){
                target = path[0].vPosition;
                target = new Vector3(target.x, transform.position.y, target.z);
                LookAtMovementDirection(target);
                
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }

        // else if(foundPathToPowerUp){
        //     // myDistanceToPowerUp = Vector3.Distance(transform.position, closestPowerUp);
        //     // playerDistanceToPowerUp = Vector3.Distance(player.transform.position, closestPowerUp);
        //     // if(myDistanceToPowerUp <= playerDistanceToPowerUp){
        //     //     MoveTowards(closestPowerUp);
        //     // } 
        //     MoveTowards(closestPowerUp);
        // }

        else if(powerUpPositions.Count > 0 && !thinking && distanceToPowerUp <= playerDistanceToPowerUp){
            targetPowerUp.y = transform.position.y;
            astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
            astarGrid.GetComponent<Pathfinding>().TargetPosition = targetPowerUp;
            List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;
            Vector3 lastPosition = targetPowerUp + transform.forward;
            Node box = new Node(false, false, lastPosition, 1, 2);

            path.Add(box);

            if(path != null && path.Count > 0){
                target = path[0].vPosition;
                target = new Vector3(target.x, transform.position.y, target.z);
                LookAtMovementDirection(target);
                
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
        }

        // If has to chase player
        else if(currentState == 0 && !thinking){
            if(myDistanceToPlayer < numExplosions){
                DropBomb();
                currentState = Random.Range(0,2);
            }
            else{
                if(HasPath(player.transform.position))
                    MoveTowards(player.transform.position);
                else{
                    if(cratePositions.Count > 0)
                        currentState = 1;
                }
            }
        }
        
        // If has to chase crate
        else if(currentState == 1 && !thinking){
            Debug.Log("Caixa " + targetCratePos + "Inimigo " + transform.position);
            Debug.Log("Distance to Crate " + myDistanceToCrate);
            if(myDistanceToCrate <= .5f){
                DropBomb();
                hasTargetCrate = false;
                myDistanceToCrate = int.MaxValue;
                currentState = Random.Range(0,2);
            }
            if(cratePositions.Count > 0){
                Debug.Log(cratePositions.Count);
                if(!hasTargetCrate){
                    GetTargetCrate();
                }
                GetDistanceToCrate();
                //MoveTowards(targetCratePos);

                targetCratePos.y = transform.position.y;
                astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
                astarGrid.GetComponent<Pathfinding>().TargetPosition = targetCratePos;
                // astarGrid.GetComponent<Pathfinding>().FindPath(transform.position, targetPos);
                List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;
                Node box = new Node(false, false, boxPosition, 1, 2);
                path.Add(box);

                if(path != null && path.Count > 0){
                    target = path[0].vPosition;
                    target = new Vector3(target.x, transform.position.y, target.z);
                    LookAtMovementDirection(target);
                    
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
            }
            else
                currentState = 0;
        }
    }

    void GetDistanceToCrate(){
        targetCratePos.y = transform.position.y;
        myDistanceToCrate = Vector3.Distance(transform.position, targetCratePos);
    }

    void GetTargetCrate(){
        ShuffleCrates();
        foreach(Vector3 cratePos in cratePositions){
            Debug.Log("To procurando caixas");
            
            Vector3[] targetCratePosNeighbors = {Vector3.forward, Vector3.back, Vector3.right, Vector3.left};

            Debug.Log(targetCratePosNeighbors.Length);

            foreach(Vector3 neighbor in targetCratePosNeighbors){
                Vector3 pos = cratePos + neighbor;
                //Debug.Log("crate" + cratePos + "neighbor" + pos);
                if (!Physics.CheckBox(pos, new Vector3(0.1f,0.1f,0.1f),new Quaternion(0,0,0,0), layerMask))
                {
                    if(HasPath(pos)){
                        //Debug.Log("Tem caminho pro " + pos);
                        hasTargetCrate = true;
                        targetCratePos = pos;
                        boxPosition = cratePos;
                        break;
                    }
                }
            }
        }
    }

    void MoveTowards(Vector3 targetPos){
        targetPos.y = transform.position.y;
        astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
        astarGrid.GetComponent<Pathfinding>().TargetPosition = targetPos;
        // astarGrid.GetComponent<Pathfinding>().FindPath(transform.position, targetPos);
        List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;

        if(path != null && path.Count > 0){
            target = path[0].vPosition;
            target = new Vector3(target.x, transform.position.y, target.z);
            LookAtMovementDirection(target);


            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    bool HasPath(Vector3 targetPos){
        astarGrid.GetComponent<Pathfinding>().StartPosition = transform.position;
        astarGrid.GetComponent<Pathfinding>().TargetPosition = targetPos;
        // astarGrid.GetComponent<Pathfinding>().FindPath(transform.position, targetPos);
        List<Node> path = astarGrid.GetComponent<AstarGrid>().FinalPath;
        return path != null && path.Count > 0 ? true : false;
    }

    bool isInDangerZone(){
        foreach(Vector3 explosionPosition in astarGrid.GetComponent<AstarGrid>().explosionPositions){
            if (Vector3.Distance(transform.position, explosionPosition) <= 1f)
                return true;
        }
        return false;
    }

    void LookAtMovementDirection(Vector3 dir){
        Vector3 lookDir = dir - transform.position;
        lookDir.y = 0f; //this is the critical part, this makes the look direction perpendicular to 'up'
        transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
    } 
    public void ShuffleCrates()
    {
        var count = cratePositions.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = cratePositions[i];
            cratePositions[i] = cratePositions[r];
            cratePositions[r] = tmp;
        }
    }
}
