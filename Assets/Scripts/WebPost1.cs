
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebPost1 : MonoBehaviour
{
    private WebReq webReq;
    private string txt = "Cancer";

    public static string id;
    //private string Url;
    // Start is called before the first frame update
    public void Awake()
    {
        StartCoroutine(PostReq("https://spool-banshee-rectangle.ngrok-free.dev/zodiac", txt));
    }

    public static IEnumerator PostReq(string url, string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("zodiac", data);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        Debug.Log("Result: " + request.result);
        
        yield return request.SendWebRequest();
        id = request.downloadHandler.text;
        Debug.Log(id);   
    }
}
