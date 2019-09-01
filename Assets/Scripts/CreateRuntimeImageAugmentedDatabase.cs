using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using TMPro;

public class CreateRuntimeImageAugmentedDatabase : MonoBehaviour
{
    public Texture2D[] images;
    ARCoreSession session;

    private void Start()
    {
        session = FindObjectOfType<ARCoreSession>();
        Debug.Log("<LOGGING> Name of the ARCoreSession: " + session.gameObject.name);
        Create();
    }

    void Create()
    {
        foreach(Texture2D image in images)
        {
            int x = session.SessionConfig.AugmentedImageDatabase.AddImage("TestImage", image);
            Debug.Log("<LOGGING> Status returned for image addition = " + x);
        }

        Debug.Log("<LOGGING> Size of Database after addition = " + session.SessionConfig.AugmentedImageDatabase.Count);
    }
}
