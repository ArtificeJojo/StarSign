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

    private const float IntervalSeconds = 20f;

    // Start is called before the first frame update
    public void Start()
    {
        WebPost1.PostReq(Url, txt);
        Url = AddToURL(Url);
        Debug.Log(Url);
        audioSource = GetComponent<AudioSource>();

        // Run PostReq immediately, then keep repeating every 20 seconds
        StartCoroutine(RepeatPostReq(Url, txt, IntervalSeconds));
    }

    private IEnumerator RepeatPostReq(string url, string data, float interval)
    {
        while (true)
        {
            // Wait for the request to fully complete before waiting out the interval,
            // so calls don't overlap/pile up if the request takes a while.
            yield return StartCoroutine(PostReq(url, data));
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator PostReq(string url, string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("prompt", data);

        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.downloadHandler = new DownloadHandlerAudioClip(url, AudioType.MPEG);

        yield return request.SendWebRequest();

        Debug.Log("Result: " + request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Request failed: " + request.error);
            yield break;
        }

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