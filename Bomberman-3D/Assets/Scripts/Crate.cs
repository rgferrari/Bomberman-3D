using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    //public Transform m_NewTransform;
    Collider m_Collider;
    Vector3 m_Point;

    bool col = false;
    int num = 0;

    public GameObject bombPowerUP;
    public GameObject ExplosionPowerUP;
    public GameObject speedPowerUP;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider>();

        anim = GetComponent<Animator>();
        //Assign the point to be that of the Transform you assign in the Inspector window
        //m_Point = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(brickCollision());

        if(col == true && num == 0)
        {
            int rand = Random.Range(0, 4);

            if(rand == 1)
            {
                Instantiate(bombPowerUP, new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), 
                                    gameObject.transform.position.y + 0.5f, Mathf.RoundToInt(gameObject.transform.position.z)),
                                    bombPowerUP.transform.rotation);
            }
            else if(rand == 2)
            {
                Instantiate(ExplosionPowerUP, new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), 
                                    gameObject.transform.position.y + 0.5f, Mathf.RoundToInt(gameObject.transform.position.z)),
                                    ExplosionPowerUP.transform.rotation);
            }
            else if(rand == 3)
            {
                Instantiate(speedPowerUP, new Vector3(Mathf.RoundToInt(gameObject.transform.position.x), 
                                    gameObject.transform.position.y + 0.5f, Mathf.RoundToInt(gameObject.transform.position.z)),
                                    speedPowerUP.transform.rotation);
            }

            num++;

            //Debug.Log ("AAAAAAAAAAA");
            anim.Play("DestroyBricks");
            Destroy(gameObject, 0.5f);
        }
    }

    void OnDestroy(){
        //Debug.Log("Brick destroyed");
    }

     public void OnTriggerEnter (Collider other)
     {
         //if (other.CompareTag("Explosion"))
         //{
             //Debug.Log ("Block" + " hit by explosion!");
             //Destroy(gameObject);
         //}
    }

    /*void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.tag == "Explosion") 
        //{
            Debug.Log ("Block hit by explosion!");
            //Destroy(gameObject);
        //}      
    }*/

    IEnumerator brickCollision()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject[] objs;
        objs = GameObject.FindGameObjectsWithTag("Explosion");
        //Debug.Log(objs.Length);

        foreach(GameObject obj in objs) {
            if(m_Collider.bounds.Contains(obj.gameObject.transform.position))
            {
                col = true;
                //Debug.Log("Colidiu");
            }
        }
    }
}
