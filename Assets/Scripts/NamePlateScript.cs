using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NamePlateScript : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GetComponent<TextMesh>().text = photonView.Owner.NickName;
        }

    }
}
