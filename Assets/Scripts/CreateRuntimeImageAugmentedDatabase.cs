using GoogleARCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateRuntimeImageAugmentedDatabase : MonoBehaviour
{
    public Texture2D[] images;
    public AugmentedImageDatabase newDatabase;
    //private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();
    public ARCoreSession session;
    int currCount = -1;
    int delay = 0;

    private void Start()
    {
        newDatabase = ScriptableObject.CreateInstance<AugmentedImageDatabase>();
        Debug.Log("<DEBUG> SCRIPT STARTED");
    }

    /*private void Update()
    {
        Session.GetTrackables<AugmentedImage>(
                m_TempAugmentedImages, TrackableQueryFilter.Updated);

        if (currCount != m_TempAugmentedImages.Count)
        {
            Debug.Log("<DEBUG> Currently Tracked items = " + currCount);
            currCount = m_TempAugmentedImages.Count;
        }

        foreach (var image in m_TempAugmentedImages)
        {
            GameObject temp = null;
            if (image.TrackingState == TrackingState.Tracking && temp == null)
            {
                // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                temp = Instantiate(sphere, anchor.transform);
                Debug.Log("<DEBUG> image Database Index = " + image.DatabaseIndex);
            }
        }
    }*/

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
        Debug.Log("<DEBUG> BUTTON PRESSED");
        foreach (var image in images)
        {
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
