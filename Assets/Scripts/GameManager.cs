using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region パブリック関数
    public void LeaveRoom()
    {
        Debug.Log("ルームから出ます。");
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        Debug.LogFormat("レベルの読み込みを開始します。プレイヤー数: {0}人", PhotonNetwork.CurrentRoom.PlayerCount);
        if (!PhotonNetwork.IsMasterClient)　Debug.LogError("マスタークライアントでないにもかかわらず、レベルを読み込もうとしています。");
        PhotonNetwork.LoadLevel("Classroom");
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
