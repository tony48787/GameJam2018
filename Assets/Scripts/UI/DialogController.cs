using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    public static string[] dialogs = new string[]
    {
        "Move Up: W, Move Left: A, Move Down: S, Move Right: D..." ,
        "Attack: left-click, Charged Attack: right-click, Switch Weapon: E..." ,
        "Place / Upgrade Tower: left-click the tile / tower...",
        "This is a game about a nameless hero fighting enemies alone...",
        "Everytime you press 'Start Wave', enemies will rush and try to kill you..." ,
        "Enemy will also try to convert your tower and shoot you. Stop them!" ,
        "Once the tower are converted, you have no way to get it back...",
        "Hope you enjoy it!"
    };

    private Vector3[] spawnLocations;

    private float width = 200f;

    private float height = 100f;

    private Image imageComponent;

    private TextMeshProUGUI textComponent;

    // Use this for initialization
    void Start () {
        imageComponent = GetComponent<Image>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
        spawnLocations = new Vector3[]
        {
            new Vector3(-GameManager.instance.horzExtent / 4, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(GameManager.instance.horzExtent / 4, 0, 0),
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (imageComponent.enabled && textComponent.enabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CloseDialog();
            }
        }
    }

    public void ShowDialog(int index)
    {
        if (index < DialogController.dialogs.Length)
        {
            imageComponent.enabled = true;
            textComponent.enabled = true;
            textComponent.text = dialogs[index];

            Vector3 newPosition = spawnLocations[index % spawnLocations.Length];

            GameObject gm = Instantiate(PrefabManager.instance.helpIcon, newPosition, Quaternion.identity);
            gm.GetComponent<HelpIcon>().current = index + 1;
        } else
        {
            CloseDialog();
            FindObjectOfType<LevelManager>().SetBtnIsActive(true);
        }
        
    }

    void CloseDialog()
    {
        imageComponent.enabled = false;
        textComponent.enabled = false;
    }
}
