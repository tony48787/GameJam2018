using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour {

    public static string[] dialogs = new string[]
    {
        "This is a game about a nameless hero fighting enemies alone...",
        "Everytime you press 'Start Wave', enemies will rush and try to kill you..." ,
        "But do not worry, you can buy tower and place it" ,
        "Be wise to spend your coins to upgrade the hero or towers..." ,
        "Enemy will try to convert your tower. Stop them!" ,
        "Once the tower are converted, you have no way to get it back...",
        "Have fun!"
    };

    private float width = 200f;

    private float height = 100f;

    private Image imageComponent;

    private TextMeshProUGUI textComponent;

    // Use this for initialization
    void Start () {
        imageComponent = GetComponent<Image>();
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
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

            Vector3 newPosition = new Vector3(
                    Random.Range(-GameManager.instance.horzExtent / 2, GameManager.instance.horzExtent / 2),
                    Random.Range(-GameManager.instance.vertExtent / 2, GameManager.instance.vertExtent / 2),
                    0);

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
