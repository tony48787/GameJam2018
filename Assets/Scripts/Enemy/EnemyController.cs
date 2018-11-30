using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 1f;
    public EnemyTarget Target;
    public int coinDrop = 10;
    public int health = 100;

    // Use this for initialization
    void Start()
    {

    }

    void FixedUpdate()
    {
//        transform.position += transform.right * Speed;
    }


    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            //look at target
            Vector3 diff = Target.gameObject.transform.position - transform.position;
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);


            //move towards target
            transform.position = Vector2.MoveTowards(transform.position, Target.gameObject.transform.position, Speed * Time.deltaTime);
        }
        
    }

    public void OnDespawn()
    {
        GameManager.instance.IncrementCoinBy(coinDrop);

        FindObjectOfType<EnemySpawner>().UpdateCurrentCountBy();

        Destroy(gameObject);
    }
}