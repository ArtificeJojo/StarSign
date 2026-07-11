using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Transform camTransform;
    public GameObject player;
    public float moveVal = 0.1f;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
            player.transform.Translate(0, 0, moveVal);
        
        if(Input.GetKey(KeyCode.A))
            player.transform.Translate(-moveVal, 0, 0);
        
        if(Input.GetKey(KeyCode.S))
            player.transform.Translate(0, 0, -moveVal);
        
        if(Input.GetKey(KeyCode.D))
            player.transform.Translate(moveVal, 0, 0);
        
        Vector3 cameraRotation = camTransform.eulerAngles;
        player.transform.rotation = Quaternion.Euler(0, cameraRotation.y, 0);
    }
    
}
