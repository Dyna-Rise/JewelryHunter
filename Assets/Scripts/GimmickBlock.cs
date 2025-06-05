using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBlock : MonoBehaviour
{
    public float length = 0.0f; //落下検知距離
    public bool isDelete; //落下後に削除する
    public GameObject deadObj; //死亡に関する当たり判定
    bool isFell; //透明消滅スタートするフラグ
    float fadeTime = 0.5f; //消滅までのフェードアウト時間
    Rigidbody2D rbody; //Rigidbody2コンポーネント
    GameObject player; //Playerの情報
    float distance; //ブロックとプレイヤーの距離

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static; //Rigidbodyのプロパティ
        deadObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            distance = Vector2.Distance(transform.position,player.transform.position);
            if(length >= distance)
            {
                if(rbody.bodyType == RigidbodyType2D.Static)
                {
                    rbody.bodyType = RigidbodyType2D.Dynamic; //Rigidbody2Dの機能復活
                    deadObj.SetActive(true);
                }
            }
        }

        if(isFell)
        {
            //透明にする
            fadeTime -= Time.deltaTime;
            Color col = GetComponent<SpriteRenderer>().color; //現状を確認
            col.a = fadeTime; //透明度を0秒にむかっているfadeTimeとリンク
            GetComponent<SpriteRenderer>().color = col; //加工したColor情報をもとに戻す
            if(fadeTime <= 0.0f)
            {
                Destroy(gameObject); //指定したオブジェクトをヒエラルキーから消滅
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDelete)
        {
            isFell = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //GimmickBlockを中心に、lengthの半径の円を描く
        Gizmos.DrawWireSphere(transform.position, length);
    }

}
