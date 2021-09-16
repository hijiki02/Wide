using System;
public struct WholeSettings
{
    //ゲームのバージョン
    public static string GameVersion { get; } = "0.21a";

    //部屋毎の最大プレイヤー数
    public static byte MaxPlayersPerRoom { get; } = 20;

    //アニメーションの遅延
    public static float AnimationDelay { get; } = 0.25f;

    //Photon Networkに接続済みかどうか
    public static Boolean isConnectedToPhotonNetwork { get; set; } = false;
}
