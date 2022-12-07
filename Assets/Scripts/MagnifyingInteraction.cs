using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagnifyingInteraction : MonoBehaviour
{
    //Define text and interactors
    public GameObject textBox;
    public TextMeshProUGUI textMessage;
    public GameObject player;
    public GameObject journal;
    public GameObject manager;

    //Define variables
    float distance = 0;
    bool activated = false;
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 3) {
                textBox.GetComponent<TextMeshProUGUI>().text = "*Press e to inspect*";
            if (Input.GetKeyDown("e") && !activated) {
                manager.GetComponent<JournalManager>().journalList.Add(journal.GetComponent<Image>());
                activated = true;
                StartCoroutine(ShowMessage("Notes added", 2));
            }
        } else {
            textBox.GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    IEnumerator ShowMessage (string message, float delay) {
        textMessage.text = message;
        textMessage.enabled = true;
        yield return new WaitForSeconds(delay);
        textMessage.enabled = false;
    }
}
