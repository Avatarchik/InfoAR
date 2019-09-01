using PolyToolkit;
using UnityEngine;
using System.Collections.Generic;

public class PolyAssetManager : MonoBehaviour
{
    List<GameObject> currentGameObjects = new List<GameObject>();
    public int currItemTotalCount = 0;
    public int currCount = 0;

    public void LoadAssetList(List<string> keywords)
    {
        currItemTotalCount = keywords.Count;

        foreach(string keyword in keywords)
        {
            Debug.Log("<DEBUG> KEYWORD = " + keyword);
            LoadAsset(keyword);           
        }
    }

    void LoadAsset(string keyword)
    {
        PolyListAssetsRequest polyListAssetsRequest = CreateRequest(keyword);
        PolyApi.ListAssets(polyListAssetsRequest, HandleRequest);
    }

    PolyListAssetsRequest CreateRequest(string keyword)
    {
        PolyListAssetsRequest req = new PolyListAssetsRequest
        {
            // Search by keyword:
            keywords = keyword,
            // Only curated assets:
            curated = true,
            // Limit complexity to medium.
            maxComplexity = PolyMaxComplexityFilter.MEDIUM,
            // Only Blocks objects.
            formatFilter = PolyFormatFilter.BLOCKS,
            // Order from best to worst.
            orderBy = PolyOrderBy.BEST,
            // Up to 20 results per page.
            pageSize = 1
        };

        return req;
    }

    void HandleRequest(PolyStatusOr<PolyListAssetsResult> result)
    {
        if (!result.Ok)
        {
            // Handle error.
            Debug.LogError("Failed to get assets. Reason: " + result.Status);
            return;
        }
        // Success. result.Value is a PolyListAssetsResult and
        // result.Value.assets is a list of PolyAssets.
        if (result.Value.assets.Count == 0)
        {
            Debug.Log("No asset found");
            return;
        }

        foreach (PolyAsset asset in result.Value.assets)
        {
            // Set the import options.
            PolyImportOptions options = PolyImportOptions.Default();
            // We want to rescale the imported mesh to a specific size.
            options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
            // The specific size we want assets rescaled to:
            options.desiredSize = 0.05f;
            // We want the imported assets to be recentered such that their centroid coincides with the origin:
            options.recenter = true;

            PolyApi.Import(asset, options, ImportAssetCallback);
        }
    }

    private void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            Debug.LogError("Failed to import asset. :( Reason: " + result.Status);
            return;
        }
        Debug.Log("Successfully imported asset: " + asset.name);

        // Here, you would place your object where you want it in your scene, and add any
        // behaviors to it as needed by your app. As an example, let's just make it
        // slowly rotate:
        result.Value.gameObject.AddComponent<Rotate>();
        result.Value.gameObject.SetActive(false);
        result.Value.gameObject.transform.parent = this.transform;
        currentGameObjects.Add(result.Value.gameObject);

        if(currItemTotalCount == currCount  + 1)
        {
            for (int i = 0; i < currentGameObjects.Count; i++)
            {
                currentGameObjects[i].transform.localPosition = GetPositionInCircle(i, currentGameObjects.Count, 0.1f);
                currentGameObjects[i].SetActive(true);
            }
        }
        else
        {
            currCount++;
        }
    }

    Vector3 GetPositionInCircle(int currIndex, int totalCount, float radius = 1f, float angleOffset = 0)
    {
        float x = radius * Mathf.Cos((2 * Mathf.PI * currIndex) / totalCount + angleOffset);
        float z = radius * Mathf.Sin((2 * Mathf.PI * currIndex) / totalCount + angleOffset);

        return new Vector3(x, 0, z);
    }

    public void DestroyPolys()
    {
        foreach(GameObject gameObject in currentGameObjects)
        {
            Debug.Log("<DEBUG> Destroying " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
