using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 1f;
    public EnemyTarget Target;


    // Use this for initialization
    void Start()
    {
        Target = FindObjectOfType<EnemyTarget>();
    }

    void FixedUpdate()
    {
//        transform.position += transform.right * Speed;
    }


    // Update is called once per frame
    void Update()
    {
        //look at target
        Vector3 diff = Target.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);


        //move towards target
        transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
    }
}