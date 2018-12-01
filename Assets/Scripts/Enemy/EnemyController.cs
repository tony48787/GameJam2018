using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed = 1f;
    public EnemyTarget Target;
    public int coinDrop = 10;
    public int health = 100;
    public float baseCoolDownTime = 4f;

    private float lastShootTime;
    private bool canCharge;
    private bool chargeCompleted;
    private bool isEnemyTargetAHero;
    public float baseForce = 200f;
    
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start()
    {
        canCharge = false;
        chargeCompleted = true;
        lastShootTime = Time.time;
        rb2d = GetComponent<Rigidbody2D>();
        isEnemyTargetAHero = Target.GetComponentInParent<HeroController>() != null;
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

            int percentage = 10;
            if (canCharge && isEnemyTargetAHero && UnityEngine.Random.Range(0, 100) >= 100 - percentage)
            {
                canCharge = false;
                StartCoroutine("WaitAndCharge");
            } else if (chargeCompleted)
            {
                transform.position = Vector2.MoveTowards(transform.position, Target.gameObject.transform.position, Speed * Time.deltaTime);
                Cooldown();
            }
        }
        
    }

    public void OnDespawn()
    {
        GameManager.instance.IncrementCoinBy(coinDrop);

        FindObjectOfType<EnemySpawner>().UpdateCurrentCountBy();

        Destroy(gameObject);
    }

    void Cooldown()
    {
        if ((Time.time - lastShootTime) > baseCoolDownTime)
        {
            canCharge = true;
        }
    }

    IEnumerator WaitAndCharge()
    {
        chargeCompleted = false;
        float force = baseForce * UnityEngine.Random.Range(1, 4);
        rb2d.Sleep();
        yield return new WaitForSeconds(0.3f);

        transform.localScale += new Vector3(0.1f, 0.1f);
        yield return new WaitForSeconds(0.3f);
        transform.localScale -= new Vector3(0.1f, 0.1f);
        yield return new WaitForSeconds(0.3f);
        transform.localScale += new Vector3(0.1f, 0.1f);
        yield return new WaitForSeconds(0.3f);
        transform.localScale -= new Vector3(0.1f, 0.1f);
        yield return new WaitForSeconds(0.3f);

        Vector3 diff = Target.gameObject.transform.position - transform.position;
        diff.Normalize();
        rb2d.WakeUp();
        rb2d.AddForce(new Vector2(diff.x * force, diff.y * force));
        yield return new WaitForSeconds(1f);

        lastShootTime = Time.time;
        chargeCompleted = true;
    }
}