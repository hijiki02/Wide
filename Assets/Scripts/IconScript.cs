using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NCMB;

public class IconScript : MonoBehaviourPunCallbacks
{
    #region 変数
    private string userID;
    #endregion

    void Start()
    {
        userID = photonView.Owner.NickName;

        NCMBFile file = new NCMBFile(userID);
        file.FetchAsync((byte[] fileData, NCMBException error) =>
        {
            if (error == null)
            {
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(fileData);

                Debug.LogFormat("アイコンファイルを取得しました。サイズ: {0}", fileData.Length);

                Rect rect = new Rect(0f, 0f, texture.width, texture.height);

                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            }
        });
    }
}
