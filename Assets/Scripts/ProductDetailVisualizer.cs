using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;
using TMPro;

public class ProductDetailVisualizer : MonoBehaviour
{
    public AugmentedImage Image;
    PolyAssetManager PolyAssetManager;
    public bool status = false;

    public void Update()
    {
        if (!status)
        {
            PolyAssetManager = gameObject.AddComponent<PolyAssetManager>();
            string[] keywords = new string[] { "perfume", "oil", "soda", "coconut", "herb" };
            List<string> items = new List<string>(keywords);
            PolyAssetManager.LoadAssetList(items);
            status = true;
        }
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {

            return;
        }

        PolyAssetManager.transform.position = Image.CenterPose.position;
    }

    public void updateStatus(TextMeshProUGUI textMesh)
    {
        textMesh.text += status.ToString() + " " + PolyAssetManager.currItemTotalCount.ToString() + " " + PolyAssetManager.currCount.ToString();
    }
}
