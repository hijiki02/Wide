using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingInputField : MonoBehaviour
{
    #region プライベートフィールド
    [Tooltip("設定のキー")]
    [SerializeField]
    private string PrefKey;
    #endregion

    #region Unityのコールバック
    void Start()
    {
        string value = string.Empty;
        InputField _inputField = GetComponent<InputField>();

        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey(PrefKey))
            {
                value = PlayerPrefs.GetString(PrefKey);
                _inputField.text = value;
            }
        }
    }
    #endregion

    #region パブリック関数
    public void SetValue(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("設定された値が空です。");
            return;
        }

        PlayerPrefs.SetString(PrefKey, value);
    }
    #endregion
}
