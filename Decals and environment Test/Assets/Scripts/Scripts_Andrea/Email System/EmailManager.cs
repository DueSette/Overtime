using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailManager : MonoBehaviour
{
    public static EmailManager singleton;
    private List<string> readEmails = new List<string>();

    private void Start()
    {
        if (singleton == null)
            singleton = this;
    }

    public void AddReadEmail(string title)
    {
        readEmails.Add(title);
    }

    public bool HasReadEmail(string title)
    {
        foreach (string readTitle in readEmails)
        {
            if (readTitle == title) { return true; }
        }
        return false;
    }
}
