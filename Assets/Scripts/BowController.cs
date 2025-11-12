using System;
using UnityEditor;
using UnityEngine;

public class BowController : MonoBehaviour
{
    public Transform player;
    //Vector3 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane
            ));
        mousePos.z = 0f;

        // Direction player to mouse
        Vector3 direction = mousePos - player.position;

        // Calculate the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Debug.Log("Angle: " + angle);

        // Rotate the bow
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip the bow when facing left...
        Vector3 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1; //flip vertically if needed
        }
        else
        {
            scale.y = 1;
        }
        transform.localScale = scale;
        Debug.Log("Mouse: " + mousePos);
    }
}
