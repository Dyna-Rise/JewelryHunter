using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CannonController : MonoBehaviour
{
    public float searchLength = 8.0f; //索敵範囲

    GameObject player; //プレイヤー情報

    public float delayTime = 3.0f; //発射インターバル
    float pastTime = 0; //経過時間

    public GameObject objPrefab; //生成するべきPrefabデータ
    public Transform gateTransform; //ゲートの位置情報

    public float fireSpeed = 4.0f; //発射速度



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //経過時間の観測
        pastTime = Time.deltaTime;

        //Playerとの距離チェックがtrueだったら（近かった）
        if (CheckLength(player.transform.position))
        {
            //時間が設定より経過していたら発射
            if(pastTime > delayTime)
            {
                pastTime = 0; //経過時間のリセット

                //砲弾の生成（対象物、位置、回転の様子）
                GameObject obj = Instantiate(objPrefab, gateTransform.position, Quaternion.identity);

                //生成した相手のRigidBodyを指定できるようにしておく
                Rigidbody2D rbody = obj.GetComponent<Rigidbody2D>();

                //キャノンの傾きを取得
                float angleZ = transform.localEulerAngles.z;
                float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
                float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
                Vector2 v = new Vector2(x, y) * fireSpeed;

                //砲弾自身のRigidbodyの力で砲弾を飛ばす
                rbody.AddForce(v, ForceMode2D.Impulse);
            }
        }
    }

    //戻り値（論理型）があるプレイヤーと砲台の距離確認
    bool CheckLength(Vector2 targetPos)
    {
        float distance = Vector2.Distance(transform.position,targetPos);
        return searchLength >= distance;
    }
}
