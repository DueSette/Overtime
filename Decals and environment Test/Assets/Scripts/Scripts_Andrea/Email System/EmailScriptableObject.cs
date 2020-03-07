using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Email", menuName = "ScriptableObjects/EmailScriptableObject", order = 1)]
public class EmailScriptableObject : ScriptableObject
{
    public string title, sender, receivers, body;
    public bool read;
}
