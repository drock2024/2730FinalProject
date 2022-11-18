using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClueInteraction : MonoBehaviour
{
    //Define text and interactors
    public GameObject textBox;
    public GameObject player;
    public GameObject journal;

    //Define variables
    float distance = 0;
    bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f") && distance < 5) {
            active = !active;
        }
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 5) {
            if (!active) {
                textBox.GetComponent<TextMeshProUGUI>().text = "*Press f to inspect*";
                journal.GetComponent<Image>().enabled = false;
            } else if (active) {
                textBox.GetComponent<TextMeshProUGUI>().text = "";
                journal.GetComponent<Image>().enabled = true;
            }
        } else {
            textBox.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}
