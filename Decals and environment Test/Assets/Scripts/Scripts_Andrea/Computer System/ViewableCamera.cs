using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewableCamera : MonoBehaviour
{
    [Header("Rendering Texture Details")]
    [SerializeField] private Camera theCamera;
    [SerializeField] private Vector2Int viewSize;
    public RenderTexture cameraView;

    [Header("Computer Connecting Details")]
    public string cameraName;
    public string cameraCode;

    // Start is called before the first frame update
    void Start()
    {
        cameraView = new RenderTexture(viewSize.x, viewSize.y, 0);
        theCamera.targetTexture = cameraView;

        // Disabling the camera until it is needed
        SetCameraEnabled(false);
    }

    public void SetCameraEnabled(bool cameraEnabled)
    {
        if (cameraEnabled)
        {
            theCamera.gameObject.SetActive(true);
        }
        else
        {
            theCamera.gameObject.SetActive(false);
        }
    }
}
