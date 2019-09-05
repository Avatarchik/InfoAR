using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DownloadManager : MonoBehaviour
{
    [System.Serializable]
    public class GetRequestResult
    {
        public List<ProductMetadata> productMetadatas;

        [System.Serializable]
        public class ProductMetadata
        {
            public string id;
            public string name;
            public string item;
            public string downloadPath;
        }

        public static GetRequestResult CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<GetRequestResult>(jsonString);
        }
    }

    public static List<Texture2D> images;
    public static GetRequestResult data;
    public static bool status = false;

    void Start()
    {
        images = new List<Texture2D>();
        StartCoroutine(FetchDataFromAPI());
    }

    IEnumerator FetchDataFromAPI()
    {
        string url = "https://us-central1-building-serverless-apps.cloudfunctions.net/getAllItems";
        yield return StartCoroutine(GetRequest(url));

        status = true;

        Debug.Log("<DEBUG> STATUS = " + status);

        foreach (var item in images)
        {
            Debug.Log(item.name + " " + item.isReadable + " " + item.format);
        }

        if (data != null)
        {
            foreach (var item in data.productMetadatas)
            {
                Debug.Log(item.name + " " + item.item);
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                data = GetRequestResult.CreateFromJSON("{\"productMetadatas\":" + webRequest.downloadHandler.text + "}");
                foreach(var item in data.productMetadatas)
                {
                    Debug.Log(item.name + "\n" + item.downloadPath);
                    yield return StartCoroutine(GetImage(item.downloadPath));
                }
            }
        }
    }

    IEnumerator GetImage(string uri)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(uri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                images.Add(texture);
            }
        }
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
