using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public float Speed = 1f;
    public GameObject Target;
    public float AttackRange = 10f;
    public HealthComponent Health;
    


    // Use this for initialization
    void Start()
    {
        Target = FindObjectOfType<EnemyTarget>().gameObject;
        Health = GetComponent<HealthComponent>();
    }

    void FixedUpdate()
    {
//        transform.position += transform.right * Speed;
    }


    // Update is called once per frame
    void Update()
    {
        DoLookAt(Target);

        float dist = Vector3.Distance(Target.transform.position, transform.position);

        if (dist <= AttackRange)
        {
            DoAttack(Target);
        }
        else
        {
            DoMove(Target);
        }
    }

    public virtual void DoMove(GameObject enemyTarget)
    {
//move towards target
        transform.position =
            Vector2.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
    }

    public virtual void DoLookAt(GameObject enemyTarget)
    {
        //look at target
        Vector3 diff = enemyTarget.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    public virtual void DoAttack(GameObject target)
    {
    }
}