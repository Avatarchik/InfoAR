using GoogleARCore;
using System.Collections.Generic;
using UnityEngine;

public class ProductDetailVisualizer : MonoBehaviour
{
    public AugmentedImage Image;
    PolyAssetManager PolyAssetManager;
    public bool status = true;
    List<ProductDetails> products;

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
        products = new List<ProductDetails>();
        products.Add(new ProductDetails("Medimix", "perfume, oil, soda, coconut, herb"));
        products.Add(new ProductDetails("Monaco", "wheat, salt, water"));
        products.Add(new ProductDetails("Lays", "Potato, salt, spices"));
        status = true;

        foreach(ProductDetails product in products)
        {
            Debug.Log("<DEBUG> Product :" + product.name);
            foreach(string item in product.items)
            {
                Debug.Log("\t\t\t<DEBUG> " + item);
            }
        }
    }

    public void Update()
    {
        if (Image == null || Image.TrackingState != TrackingState.Tracking)
        {
            return;
        }

        if(products != null)
        {
            if(products.Count == 0)
            {
                products.Add(new ProductDetails("Medimix", "perfume, oil, soda, coconut, herb"));
                products.Add(new ProductDetails("Monaco", "wheat, salt, water"));
                products.Add(new ProductDetails("Lays", "Potato, salt, spices"));
            }
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
        if (products == null)
        {
            Debug.Log("<DEBUG> product is null");
        }
        else if(products.Count == 0)
        {
            Debug.Log("<DEBUG> product count is 0");
        }
        else
        {
            Debug.Log("<DEBUG> Product = " + products[Image.DatabaseIndex].name);
            PolyAssetManager.LoadAssetList(products[Image.DatabaseIndex].items);
            status = false;
        }
    }

    public void RemovePolyAssetManager()
    {
        PolyAssetManager.DestroyPolys();
    }
}
