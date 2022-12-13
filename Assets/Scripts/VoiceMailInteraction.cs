using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VoiceMailInteraction : MonoBehaviour
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
    int messageCount = 0;

    //Define Audio Clips
    public AudioClip finance;
    public AudioClip principal;
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 3) {
                textBox.GetComponent<TextMeshProUGUI>().text = "*Press e to inspect*";
            if (Input.GetKeyDown("e") && !activated) {
                manager.GetComponent<JournalManager>().journalList.Add(journal.GetComponent<Image>());
                activated = true;
                PlayMessage();
                JournalManager manageScript = manager.GetComponent<JournalManager>();
                manageScript.ScribbleNotes();
                StartCoroutine(ShowMessage("Notes added", 2));
                JournalManager.momFound = true;
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

    void PlayMessage() {
        if (messageCount == 0) {
            AudioSource audioPlayer = GetComponent<AudioSource>();
            audioPlayer.clip = finance;
            audioPlayer.Play();
            messageCount = 1;
        } else if (messageCount == 1) {
            AudioSource audioPlayer = GetComponent<AudioSource>();
            audioPlayer.clip = principal;
            audioPlayer.Play();
            messageCount = 1;
        }
        
    }
}
