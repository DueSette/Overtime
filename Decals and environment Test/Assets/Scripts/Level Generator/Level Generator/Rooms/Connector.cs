using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public Directions connectorDirection;
    public bool connected;

    public void UpdateConnector()
    {
        int rotation = (int)this.transform.eulerAngles.y;
        if (rotation < 0)
        {
            rotation += 360;
        }
        if (rotation >= 360)
        {
            rotation -= 360;
        }
        this.connectorDirection = (Directions)(rotation);
    }

    private void OnValidate()
    {
        UpdateConnector();
    }
    private void OnDrawGizmos()
    {
        // Drawing Pre Defined Connector Points
        Gizmos.color = Color.red;
        // Connection Direction
        Vector3 arrowTipFront = this.transform.position + (this.transform.forward * 0.5f);
        Vector3 arrowTipLeft = this.transform.position + (this.transform.forward * 0.25f) + (this.transform.right * 0.25f);
        Vector3 arrowTipRight = this.transform.position + (this.transform.forward * 0.25f) - (this.transform.right * 0.25f);
        Vector3 arrowEnd = this.transform.position - (this.transform.forward * 0.5f);

        Gizmos.DrawLine(arrowTipFront, arrowTipRight);
        Gizmos.DrawLine(arrowTipFront, arrowTipLeft);
        Gizmos.DrawLine(arrowTipRight, arrowTipLeft);
        Gizmos.DrawLine(arrowTipFront, arrowEnd);

        // Door Frame
        Vector3 rightSide = this.transform.position + (this.transform.right * 0.5f);
        Vector3 leftSide = this.transform.position - (this.transform.right * 0.5f);

        Gizmos.DrawLine((rightSide), (rightSide + (Vector3.up * 2)));
        Gizmos.DrawLine(leftSide, (leftSide + (Vector3.up * 2)));
        Gizmos.DrawLine((rightSide + (Vector3.up * 2)), (leftSide + (Vector3.up * 2)));
    }
}