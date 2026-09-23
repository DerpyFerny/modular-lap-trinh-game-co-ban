using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GettingVcam : MonoBehaviour
{
    public CinemachineConfiner vCam;
    private GameObject camOBJ;

    // Subscribe to the event when this script is enabled
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribe when disabled to prevent memory leaks
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This function will automatically run every time a scene finishes loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (vCam == null)
        {
            camOBJ = GameObject.Find("Virtual Camera");
            if (camOBJ != null)
            {
                vCam = camOBJ.GetComponent<CinemachineConfiner>();
            }
        }

        if (vCam == null)
        {
            Debug.LogWarning("Virtual Camera not found!");
            return;
        }

        GameObject camBound = GameObject.FindWithTag("CamBound");
        if (camBound != null)
        {
            Debug.Log("Binding camBoundBox in scene: " + scene.name);
            vCam.m_BoundingShape2D = camBound.GetComponent<PolygonCollider2D>();

            // Cinemachine caches confiner bounds internally — this forces a refresh
            vCam.InvalidatePathCache();
        }
        else
        {
            Debug.LogWarning("No CamBound found in scene: " + scene.name);
        }
    }
}
