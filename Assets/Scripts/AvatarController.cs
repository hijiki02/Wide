using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class AvatarController : MonoBehaviourPunCallbacks
{
    private Camera cam;
    private Vector3 camOffset;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camOffset = new Vector3(0f, 12.5f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            transform.Translate(15f * Time.deltaTime * input.normalized);

            cam.transform.position = transform.position + camOffset;
        }
    }
}
