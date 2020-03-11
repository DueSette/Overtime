using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailManager : MonoBehaviour
{
    private static List<string> readEmails = new List<string>();

    private void OnEnable()
    {
        ComputerScript.ReadEmailEvent += AddToList;
    }
    private void OnDisable()
    {
        ComputerScript.ReadEmailEvent -= AddToList;
    }

    public static bool HasReadEmail(string title)
    {
        foreach (string readTitle in readEmails)
        {
            if (readTitle == title) { return true; }
        }
        return false;
    }

    private void AddToList(string s)
    {
        print(readEmails.Count);

        foreach (string readTitle in readEmails)
        {
            if (readTitle == s) { return; }
        }
        readEmails.Add(s);
    }
}
