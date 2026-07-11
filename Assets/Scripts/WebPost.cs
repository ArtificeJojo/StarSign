using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebPost : MonoBehaviour
{
    private WebReq webReq;
    private string txt = "Player is going the wrong way";
    //private string Url;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PostReq("http://172.20.10.4:5000/barnum", txt));
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
        //DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
        //AudioClip audioClip = 
        /*
        DownloadHandlerAudioClip downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);
        request.downloadHandler = downloadHandler;
        AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Audio file was made");
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                Debug.Log("Audio Played");
            }
        }
        */
        
    }
}
