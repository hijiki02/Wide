using Photon.Pun;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AvatarController : MonoBehaviourPunCallbacks
{
    #region 変数
    private Rigidbody rb;
    private GameObject camObject;
    private Camera cam;
    private float h, v;
    #endregion

    #region プライベートフィールド
    [Tooltip("カメラの距離")]
    [SerializeField]
    private float camOffset;

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
        rb = GetComponent<Rigidbody>();
        camObject = GameObject.Find("PlayerCamera");
        cam = camObject.GetComponent<Camera>();

        cam.transform.position += camOffset * transform.forward;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            v = CrossPlatformInputManager.GetAxisRaw("Vertical");

            rb.velocity = v * speed * transform.forward;
            transform.rotation *= Quaternion.AngleAxis(movingTurnSpeed * h * Time.deltaTime, Vector3.up);
        }
    }
    #endregion
}
