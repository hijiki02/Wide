using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System;
using NCMB;
using SFB;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region プライベートフィールド
    [Tooltip("Recorderを含むGameObject")]
    [SerializeField]
    private GameObject recorderObject;

    [Tooltip("接続ステータス")]
    [SerializeField]
    private GameObject label;

    [Tooltip("入力フォーム")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("設定パネル")]
    [SerializeField]
    private GameObject settingsPanel;

    [Tooltip("進捗ラベル")]
    [SerializeField]
    private GameObject progressLabel;
    #endregion

    #region 変数
    private string roomID = "Default Room";
    #endregion

    #region Unityのコールバック
    void Awake()
    {
        Debug.LogFormat("ゲームのバージョン: {0}", WholeSettings.GameVersion);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        InitPlayerID();

        if (!WholeSettings.isConnectedToPhotonNetwork)
        {
            SetMicrophoneDevice();
            Connect();
            WholeSettings.isConnectedToPhotonNetwork = true;
        }
    }
    #endregion

    #region パブリック関数
    public void SetRoomID(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("エラー: ルームIDが入力されていません。");
            return;
        }
        else
        {
            roomID = value;
        }
    }

    public void EnterRoom()
    {
        Debug.Log("入室を開始します。");
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = WholeSettings.MaxPlayersPerRoom;
        PhotonNetwork.JoinOrCreateRoom(roomID, roomOptions, TypedLobby.Default);
    }

    public void Settings(bool isActive)
    {
        if (isActive) settingsPanel.SetActive(true);
        else settingsPanel.SetActive(false);
    }

    public void UploadIcon()
    {
        StandaloneFileBrowser.OpenFilePanelAsync("Open File", "", "", false, (string[] paths) => {
            foreach (string path in paths)
            {
                Image image = Image.FromFile(path);
                byte[] imageByte = (byte[])new ImageConverter().ConvertTo(image, typeof(byte[]));

                NCMBFile file = new NCMBFile(PlayerPrefs.GetString("userID"), imageByte);
                file.SaveAsync();
            }
        });
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
        #endif
    }
    #endregion

    #region 関数
    private void InitPlayerID()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("userID")))
        {
            NCMBObject userClass = new NCMBObject("Users");
            string guid = Guid.NewGuid().ToString();

            PlayerPrefs.SetString("userID", guid);
            userClass["userID"] = guid;
            userClass.SaveAsync();
        }
    }

    private void SetMicrophoneDevice()
    {
        Debug.Log("マイクの取得を開始します。");
        Recorder recorder = recorderObject.GetComponent<Recorder>();

        var enumerator = recorder.MicrophonesEnumerator;

        if (enumerator.IsSupported)
        {
            var devices = enumerator.Devices.GetEnumerator();
            devices.MoveNext();

            Debug.LogFormat("マイクを取得しました。デバイス: {0}", devices.Current);
            recorder.MicrophoneDevice = devices.Current;
        }
    }

    private void Connect()
    {
        Debug.Log("Photon Networkへの接続を開始します。");

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("すでにPhoton Networkと接続しています。");
        } else
        {
            PhotonNetwork.GameVersion = WholeSettings.GameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
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
        label.SetActive(false);
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
    #endregion
}
