using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using NCMB;

[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
    NCMBObject userClass;
    string userID;

    #region MonoBehaviour CallBacks
    void Start()
    {
        if (PlayerPrefs.HasKey("userID"))
        {
            userClass = new NCMBObject("Users");
            userID = PlayerPrefs.GetString("userID");

            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Users");
            query.WhereEqualTo("userID", userID);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
                if (e == null)
                {
                    foreach (NCMBObject obj in objList)
                    {
                        Debug.Log("objectId:" + obj.ObjectId);
                        userClass.ObjectId = obj.ObjectId;
                    }

                    userClass.FetchAsync();
                }
            });

            InputField inputField = this.GetComponent<InputField>();

            if (inputField != null)
            {
                inputField.text = (string)userClass["friendlyName"];
            }
        }
    }
    #endregion

    #region Public Methods
    public void SetFriendlyName(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            userClass["friendlyName"] = value;
            userClass["userID"] = userID;

            userClass.SaveAsync();
        }
    }
    #endregion
}
