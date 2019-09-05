using GoogleARCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateRuntimeImageAugmentedDatabase : MonoBehaviour
{
    public AugmentedImageDatabase newDatabase;
    public ARCoreSession session;
    int currCount = -1;
    int delay = 0;

    private void Start()
    {
        newDatabase = ScriptableObject.CreateInstance<AugmentedImageDatabase>();
        Debug.Log("<DEBUG> SCRIPT STARTED");
    }

    private void Update()
    {
        if (delay == 50)
        {
            Create();
            Debug.Log("<DEBUG> CALLING CREATE()");
            delay = 51;
        }
        else if(delay < 51)
        {
            delay += 1;
            Debug.Log("<DEBUG> CURRENT FRAME = " + delay);
        }
    }

    public void Create()
    {
        foreach (var image in DownloadManager.images)
        {
            Debug.Log("<DEBUG> CREATE " + image.name + " " + image.isReadable + " " + image.format);
            int x = newDatabase.AddImage(image.name, image);
            Debug.Log("<DEBUG> Val returned on adding Image = " + x);
        }

        Debug.Log("<DEBUG> Databse count = " + newDatabase.Count);
        if (session != null)
        {
            session.SessionConfig.AugmentedImageDatabase = newDatabase;
            Debug.Log("<DEBUG> NEW DATABASE SET");
        }
        else
        {
            Debug.Log("<DEBUG> SESSION IS NULL");
        }
    }
}
