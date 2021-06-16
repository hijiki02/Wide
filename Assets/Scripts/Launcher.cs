using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region プライベートフィールド
    [Tooltip("入力フォーム")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("進捗ラベル")]
    [SerializeField]
    private GameObject progressLabel;
    #endregion

    #region Unityのコールバック
    void Awake()
    {
        Debug.LogFormat("ゲームのバージョン: {0}", WholeSettings.gameVersion);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion

    #region パブリック関数
    public void Connect()
    {
        Debug.Log("Photon Networkへの接続を開始します。");
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Photon Networkに接続できません。");
        } else
        {
            PhotonNetwork.GameVersion = WholeSettings.gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion

    #region PUNのコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターに接続されました。");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに入りました。");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに入りました。");
        PhotonNetwork.LoadLevel("Classroom");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("切断されました。理由: {0}", cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("ランダム入室に失敗しました。ルームが存在していない可能性があります。");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = WholeSettings.maxPlayersPerRoom });
    }
    #endregion
}
