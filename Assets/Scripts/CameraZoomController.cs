using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public float scroll;        //Input axis of your scroll wheel.
    public float addedSpeed;    //Input axis doesnt have enough power to make camera zoom properly so we add it
    public float cameraSpeed;   //Lerp speed (PLAY AROUND WITH IT!)

    void Update()
    {
        //If a scroll happens zoom or zoom out:
        scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += scroll + addedSpeed;


        //IF youre zooming (IN) then lerp and zoom to your mouse position
        if (scroll < 0)
        {
            Camera.main.transform.position = Vector2.Lerp(Camera.main.transform.position,
                                                          Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                                          cameraSpeed * Time.deltaTime);


            //KEEP THE Z POSITION BEHIND YOUR OBJECTS!
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
                                                         Camera.main.transform.position.y,
                                                         -20);
        }

        //If scroll > 0 then just zoom out but leave your camera at the position that it is. YOU CAN CHANGE THIS
    }
}
