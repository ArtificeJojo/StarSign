using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebPost : MonoBehaviour
{
    private WebReq webReq;
    private string txt = "introduce yourself";
    //private string Url;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PostReq("http://172.20.10.4:5000/ai", txt));
        //webReq.Start();
    }

    private IEnumerator PostReq(string url, string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("prompt", data);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        Debug.Log("Result: " + request.result);

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
