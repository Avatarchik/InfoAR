using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePolyScript : MonoBehaviour
{
    public ProductDetailVisualizer AugmentedImageVisualizerPrefab;

    PolyAssetManager PolyAssetManager;
    public void onClickButton()
    {
        var product = (ProductDetailVisualizer)Instantiate(AugmentedImageVisualizerPrefab, this.transform);
    }
}
