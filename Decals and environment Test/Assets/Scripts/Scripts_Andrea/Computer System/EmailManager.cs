using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailManager : MonoBehaviour
{
    private static List<string> readEmails = new List<string>();

    private void OnEnable()
    {
        ComputerEmailSystem.ReadEmailEvent += AddToList;
    }
    private void OnDisable()
    {
        ComputerEmailSystem.ReadEmailEvent -= AddToList;
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
        foreach (string readTitle in readEmails)
        {
            if (readTitle == s) { return; }
        }
        readEmails.Add(s);
    }
}
