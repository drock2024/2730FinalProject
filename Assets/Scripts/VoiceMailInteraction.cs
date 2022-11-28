using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoiceMailInteraction : MonoBehaviour
{
    //Define text and interactors
    public GameObject textBox;
    public GameObject player;
    public GameObject journal;
    public GameObject manager;

    //Define variables
    float distance = 0;
    bool active = false;
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 5) {
                textBox.GetComponent<TextMeshProUGUI>().text = "*Press e to inspect*";
            if (Input.GetKeyDown("e")) {
                manager.GetComponent<JournalManager>().journalList.Add(journal.GetComponent<Image>());
            }
        } else {
            textBox.GetComponent<TextMeshProUGUI>().text = "";
        }
    }
}