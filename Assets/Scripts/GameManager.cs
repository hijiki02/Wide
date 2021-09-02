using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region プライベートフィット
    [SerializeField]
    private GameObject roomIdObject;
    #endregion

    #region Unityのコールバック
    private void Start()
    {
        Debug.Log("アバターの生成を開始します。");
        Vector3 position = new Vector3(Random.Range(-90f, 90f), 0.25f, Random.Range(-90f, 90f));
        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);

        roomIdObject.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
    }
    #endregion

    #region 関数
    public void LeaveRoom()
    {
        Debug.Log("退室を開始します。");
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        Debug.LogFormat("レベルの読み込みを開始します。プレイヤー数: {0}人", PhotonNetwork.CurrentRoom.PlayerCount);
        if (!PhotonNetwork.IsMasterClient)　Debug.LogError("マスタークライアントでないにもかかわらず、レベルを読み込もうとしています。");
    }
    #endregion

    #region Photonのコールバック
    public override void OnLeftRoom()
    {
        Debug.Log("退室しました。");
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("{0}が入室しました。", other.NickName);
        if (PhotonNetwork.IsMasterClient)　LoadArena();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("{0}が退室しました。", other.NickName);
        if (PhotonNetwork.IsMasterClient)　LoadArena();
    }
    #endregion
}
