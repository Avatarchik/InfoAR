using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ProductDetailVisualizer : MonoBehaviour
{
    public AugmentedImage Image;
    PolyAssetManager PolyAssetManager;
    public bool status = true;
    ProductDetails product;

    class ProductDetails
    {
        public string name;
        public List<string> items;

        public ProductDetails(string productName, string keywords)
        {
            name = productName;
            string[] separatedKeywords = keywords.Split(',');
            items = new List<string>(separatedKeywords);
        }
    }

    private void Start()
    {
        PolyAssetManager = gameObject.AddComponent<PolyAssetManager>();
        status = true;
    }

    public void Update()
    {
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            return;
        }
 
        if (status)
        {
            Debug.Log("<DEBUG> STATUS = " + status);
            SetPolyAssetManager();
            Debug.Log("<DEBUG> STATUS = " + status);
        }

        PolyAssetManager.transform.position = Image.CenterPose.position;
    }

    public void SetPolyAssetManager()
    {
        Debug.Log("<DEBUG> Image Database Index = " + Image.DatabaseIndex);
        product = new ProductDetails(
            DownloadManager.data.productMetadatas[Image.DatabaseIndex].name, 
            DownloadManager.data.productMetadatas[Image.DatabaseIndex].item
            );
        if (product == null)
        {
            Debug.Log("<DEBUG> product is null");
        }
        else
        {
            Debug.Log("<DEBUG> Product = " + product.name);
            PolyAssetManager.LoadAssetList(product.items);
            status = false;
        }
    }

    public void RemovePolyAssetManager()
    {
        PolyAssetManager.DestroyPolys();
    }
}
