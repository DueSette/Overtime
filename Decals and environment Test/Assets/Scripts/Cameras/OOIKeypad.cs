using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOIKeypad : ObjectOfInterest
{
       public override void FocusCamera()
    {
        base.FocusCamera();
        Debug.Log("The Changed class was called");
    }
}
