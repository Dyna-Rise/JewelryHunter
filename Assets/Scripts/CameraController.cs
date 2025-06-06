using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float leftLimit; //カメラの左の移動の限界
    public float rightLimit; //カメラの右の移動の限界
    public float topLimit; //カメラの上の移動の限界
    public float bottomLimit; //カメラの下の移動の限界

    public bool isForceScrollX; // X 強制スクロールフラグ
    public float forceScrollSpeedX = 0.5f; //スクロールスピード

    public bool isForceScrollY; // Y 強制スクロールフラグ
    public float forceScrollSpeedY = 0.5f; //スクロールスピード

    public GameObject subScreen; //サブスクリーン

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float currentX = player.transform.position.x; //プレイヤーのX座標を取得
            float currentY = player.transform.position.y; //プレイヤーのY座標を取得

            //Xが強制スクロールならば、froceScrollSpeedXの分だけ自動で加算されている
            if (isForceScrollX) currentX = transform.position.x + (forceScrollSpeedX * Time.deltaTime);
            //Yが強制スクロールならば、froceScrollSpeedYの分だけ自動で加算されている
            if (isForceScrollY) currentY = transform.position.y + (forceScrollSpeedY * Time.deltaTime);

            //リミットで止まる
            if (currentX < leftLimit)
            {
                currentX = leftLimit;
            }
            else if (currentX > rightLimit)
            {
                currentX = rightLimit;
            }

            if (currentY < bottomLimit)
            {
                currentY = bottomLimit;
            }
            else if (currentY > topLimit)
            {
                currentY = topLimit;
            }

            //XとYはプレイヤーと同じ、Z軸は距離感を保つ
            transform.position = new Vector3(currentX, currentY, transform.position.z);

            //サブスクリーンはカメラより鈍い動きで連動させる
            if (subScreen != null)
            {
                subScreen.transform.position = new Vector3(currentX / 2.0f,subScreen.transform.position.y, subScreen.transform.position.z);
            }

        }
    }
}
