using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AvatarController : MonoBehaviourPunCallbacks
{
    #region 変数
    private Camera cam;
    private Rigidbody rb;
    private float h, v;
    #endregion

    #region プライベートフィールド
    [Tooltip("カメラの距離")]
    [SerializeField]
    private Vector3 camOffset = new Vector3(0f, 12.5f, 12.5f);

    [Tooltip("移動速度")]
    [SerializeField]
    private float speed = 25f;

    [Tooltip("回転速度")]
    [SerializeField]
    private float movingTurnSpeed = 100;
    #endregion

    #region Unityのコールバック
    private void Start()
    {
        if (photonView.IsMine)
        {
            GameObject camObject = transform.Find("Camera").gameObject;
            camObject.SetActive(true);

            cam = camObject.GetComponent<Camera>();
            rb = GetComponent<Rigidbody>();

            // TODO: Avatarとカメラの距離をユーザーが設定できるようにする
            //cam.transform.position = camOffset;
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            rb.velocity = -v * speed * transform.forward;
            transform.rotation *= Quaternion.AngleAxis(movingTurnSpeed * h * Time.deltaTime, Vector3.up);
        }
    }
    #endregion
}
