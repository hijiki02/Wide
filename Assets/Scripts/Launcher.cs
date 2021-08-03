using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region 変数
    private Recorder recorder;
    #endregion

    #region プライベートフィールド
    [Tooltip("Recorderを含むGameObject")]
    [SerializeField]
    private GameObject recorderObject;

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
        Debug.LogFormat("ゲームのバージョン: {0}", WholeSettings.GameVersion);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        recorder = recorderObject.GetComponent<Recorder>();
        SetMicrophoneDevice();

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }
    #endregion

    #region パブリック関数
    public void SetMicrophoneDevice()
    {
        Debug.Log("マイクの取得を開始します。");
        var enumerator = recorder.MicrophonesEnumerator;

        if (enumerator.IsSupported)
        {
            var devices = enumerator.Devices.GetEnumerator();
            devices.MoveNext();

            Debug.LogFormat("マイクを取得しました。デバイス: {0}", devices.Current);
            recorder.MicrophoneDevice = devices.Current;
        }
    }

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
            PhotonNetwork.GameVersion = WholeSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void EnterRoom()
    {
        Debug.Log("ランダム入室を開始します。");
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion

    #region PUNのコールバック
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターに接続されました。");
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに入りました。");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("入室しました。");
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
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = WholeSettings.MaxPlayersPerRoom });
    }
    #endregion
}
