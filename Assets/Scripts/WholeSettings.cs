using System;
public struct WholeSettings
{
    //ゲームのバージョン
    public static string GameVersion { get; } = "0.11a";

    //部屋毎の最大プレイヤー数
    public static byte MaxPlayersPerRoom { get; } = 4;

    //フォントサイズ
    public static int FontSize { get; set; } = 14;
}
