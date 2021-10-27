using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NCMB;
using UnityEngine.UI;

public class NamePlateScript : MonoBehaviourPunCallbacks
{
    #region 変数
    private string userID;
    private NCMBObject userClass;
    #endregion

    void Start()
    {
        if (photonView.IsMine)
        {
            photonView.Owner.NickName = PlayerPrefs.GetString("userID");
            gameObject.SetActive(false);
        }
        else
        {
            userID = photonView.Owner.NickName;
            userClass = new NCMBObject("Users");

            Debug.LogFormat("userID: {0}", userID);

            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Users");
            query.WhereEqualTo("userID", userID);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e == null)
                {
                    foreach (var obj in objList)
                    {
                        Debug.Log("objectId:" + obj.ObjectId);
                        userClass.ObjectId = obj.ObjectId;

                        userClass.FetchAsync((NCMBException e) => {
                            if (e == null)
                            {
                                string friendlyName = (string)userClass["friendlyName"];
                                GetComponent<TextMesh>().text = friendlyName;
                            }
                        });
                    }

                }
            });
        }
    }
}
