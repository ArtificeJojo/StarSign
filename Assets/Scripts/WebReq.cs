using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WebReq : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(GetReq("http://172.20.10.4:5000/ai"));
    }
    
    IEnumerator GetReq(string url)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
        yield return request.SendWebRequest();
        
        if (request.isNetworkError || request.isHttpError)
            Debug.Log("Error: " + request.error);
        else
        {
            Debug.Log("Successfully Received:" + request.downloadHandler + url);
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                Debug.Log("Audio Played");
                /*
                string save = Path.Combine(Application.dataPath, "audioClip.mp3");
                File.WriteAllBytes(save, request.downloadHandler.data);
                Debug.Log("Audio Saved to " + save);
                */
            }
            else
                Debug.Log("No Audio Found or Download Error");
            
        }
            
            
        
    }
}
