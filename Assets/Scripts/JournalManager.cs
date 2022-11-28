using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalManager : MonoBehaviour
{
    // Define stack for journal pages
    public List<Image> journalList = new List<Image>();
    GameObject cover;
    public int page = 0;

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
        if (Input.GetKeyDown("right")) {
            if (page < journalList.Count - 1) {
                journalList[page].enabled = false;
                page++;
                journalList[page].enabled = true;
            }
        }
        if (Input.GetKeyDown("left")) {
            if (page > 0) {
                journalList[page].enabled = false;
                page--;
                journalList[page].enabled = true;
            }
        }
    }
}
