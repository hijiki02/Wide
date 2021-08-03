using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target; //<! Playerを中心に回転するのに使用
	public float speedY = 60f;
	public float maxAngle = 45f;
	private float anglePerSec = 360f; //<! 角速度（オイラー）
	private float hitDir = 0f;
	private bool Impenetrable = false; //<! カメラが通行不能な場合はtrue

	// Update is called once per frame
	void Update () {
		//カーソルのロック
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		//X軸の監視
		Impenetrable = Input.GetAxis ("Mouse X") * hitDir > 0;//<! 入力方向と壁のある方向が一致する場合は0より大きな正の数になるため、これで通行可能か判定（Rigidbodyで動かしていないためこのように判定）
		if(Impenetrable){Debug.Log("カメラ接触中"); return;}
		MouseX ();
		//Y軸の監視
		MouseY ();
	}

	void MouseX (){
		//X方向の入力でtargetを中心に回転
		float angle = Input.GetAxis ("Mouse X") * anglePerSec * Time.deltaTime;
		transform.RotateAround(target.position, Vector3.up ,angle);
	}

	void MouseY (){
		//カメラの回転をオイラーで取得（角度の限度をオイラーで判定するため）
		Vector3 angles = transform.eulerAngles;
		//x軸周りの回転にマウスの上下の動きを減算（MouseYは上が負、下が正）
		angles.x -= Input.GetAxis ("Mouse Y") * speedY * Time.deltaTime;
		//減算後の角度を限度角度内に収める
		float limitUpper = 360 - maxAngle;
		//仰角の限度
		if(angles.x < limitUpper && angles.x > 180f){angles.x = limitUpper;}
		//俯角の限度
		if(angles.x > maxAngle && angles.x < 180f){angles.x = maxAngle;}
		//カメラの回転
		transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z);
	}

	void OnTriggerEnter(Collider other){
		hitDir = Input.GetAxis("Mouse X");
	}

	void OnTriggerExit(Collider other){
		hitDir = 0;
	}
}