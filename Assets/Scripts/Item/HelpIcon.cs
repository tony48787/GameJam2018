using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpIcon : MonoBehaviour {

    private DialogController dialogController;

    private CircleCollider2D circleCollider2D;

    public int current = 0;

    // Use this for initialization
    void Start () {
        circleCollider2D = GetComponent<CircleCollider2D>();
        dialogController = FindObjectOfType<DialogController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.CompareTag("Player"))
        {

            dialogController.ShowDialog(current);

            Destroy(gameObject);
        }
    }
}
