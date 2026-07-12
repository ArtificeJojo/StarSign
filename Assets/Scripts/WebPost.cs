
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebPost : MonoBehaviour
{
    private WebReq webReq;
    private string txt = "Player is going the wrong way";
    public string Id;
    private string Url = "https://spool-banshee-rectangle.ngrok-free.dev/barnum";
    public AudioSource audioSource;

    public WebPost1 webPost1; 
    // Start is called before the first frame update
    public void Start()
    {
        WebPost1.PostReq(Url, txt);
        Url = AddToURL(Url);
        Debug.Log(Url);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PostReq(Url, txt));
        //webReq.Start();
    }

    private IEnumerator PostReq(string url, string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("prompt", data);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        Debug.Log("Result: " + request.result);

        request.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);
        yield return request.SendWebRequest();
        
        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            Debug.Log("Audio Played");
        }
    }

    public static string AddToURL(string url)
    {
        string result = url + "/" + WebPost1.id;
        return result;
    }
}
