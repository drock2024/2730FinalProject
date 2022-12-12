using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalReport : MonoBehaviour
{
    //Define dropdowns
    public TMP_Dropdown culpritBox;
    public TMP_Dropdown causeBox;
    public TMP_Dropdown moneyBox;

    void Start()
    {
        //Unlock the cursor
        Cursor.lockState = CursorLockMode.None;

        //Add options based on clue discovery
        if (JournalManager.momFound) {
            List<string> mom = new List<string> {"Hailey Plotwell"};
            culpritBox.AddOptions(mom);
        }

        if (JournalManager.daughterFound) {
            List<string> daughter = new List<string> {"Plotwell daughter"};
            culpritBox.AddOptions(daughter);
        }

        if (JournalManager.sonFound) {
            List<string> son = new List<string> {"Plotwell son"};
            culpritBox.AddOptions(son);
        }
    }

}
