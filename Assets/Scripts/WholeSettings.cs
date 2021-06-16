using System;
public struct WholeSettings
{
    //ゲームのバージョン
    public static string gameVersion { get; } = "1.0";

    //部屋毎の最大プレイヤー数
    public static byte maxPlayersPerRoom { get; } = 4;
}
