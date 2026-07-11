using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebReq : MonoBehaviour
{
    //public string Url;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetReq("http://10.32.241.86:5000/"));
    }
    
    IEnumerator GetReq(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        
        if (request.isNetworkError || request.isHttpError)
            Debug.Log("Error: " + request.error);
        else
            Debug.Log("Successfully Received:" + request.downloadHandler.text);
            
        
    }
}
