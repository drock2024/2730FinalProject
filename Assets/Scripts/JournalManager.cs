using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JournalManager : MonoBehaviour
{
    // Define stack for journal pages
    public List<Image> journalList = new List<Image>();
    GameObject cover;
    public int page = 0;
    //Checks for culprits
    public static bool momFound = false;
    public static bool sonFound = false;
    public static bool daughterFound = false;
    public static bool natureFound = false;
    //Audio
    public AudioClip scribble;
    public AudioClip pageTurn;

    // Start is called before the first frame update
    void Start() {
        cover = GameObject.Find("Journal");
        journalList.Add(cover.GetComponent<Image>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r")) {
            journalList[page].enabled = !journalList[page].enabled;
        }
        if (Input.GetKeyDown("m")) {
            if (page < journalList.Count - 1) {
                journalList[page].enabled = false;
                page++;
                journalList[page].enabled = true;
                AudioSource audioPlayer = GetComponent<AudioSource>();
                audioPlayer.clip = pageTurn;
                audioPlayer.Play();
            }
        }
        if (Input.GetKeyDown("n")) {
            if (page > 0) {
                journalList[page].enabled = false;
                page--;
                journalList[page].enabled = true;
                AudioSource audioPlayer = GetComponent<AudioSource>();
                audioPlayer.clip = pageTurn;
                audioPlayer.Play();
            }
        }

        if (Input.GetKeyDown("p")) {
            SceneManager.LoadScene("FinalReport");
        }
    }

    public void ScribbleNotes() {
        AudioSource audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = scribble;
        audioPlayer.Play();
    }
}
