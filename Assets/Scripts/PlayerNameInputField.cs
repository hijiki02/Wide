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
                    Debug.Log("objectId:" + objList[0].ObjectId);
                    userClass.ObjectId = objList[0].ObjectId;

                    userClass.FetchAsync((NCMBException e) => {
                        if (e == null)
                        {
                            InputField inputField = GetComponent<InputField>();
                            inputField.text = (string)userClass["friendlyName"];
                        }
                    });
                }
            });
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
