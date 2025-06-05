using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 3.0f; //X方向への移動距離
    public float moveY = 3.0f;　//Y方向への移動距離
    public float times = 3.0f; //何秒かけて移動するか
    public float wait = 1.0f; //反転するまでのインターバル
    float distance; //開始地点と移動予定地点の差
    float secondsDistance; //1秒あたりの移動距離
    float framsDistance; //とある1フレームあたりにおける移動距離
    float movePercentage = 0; //目的までの移動進捗(割合)

    bool isCanMove = true; //動いてOKかのフラグ
    Vector3 startPos; //ブロックの初期位置
    Vector3 endPos; //移動後の予定位置
    bool isReverse; //方向反転フラグ

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = new Vector2(startPos.x + moveX,startPos.y + moveY);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCanMove) return;

        distance = Vector2.Distance(startPos, endPos);
        secondsDistance = distance / times; //1秒あたりに進むべき距離
        framsDistance = secondsDistance * Time.deltaTime;
        movePercentage += framsDistance / distance;

        if (!isReverse) { 
            //線形補間メソッド 第3引数に指定した進捗率の地点にいかせる
            transform.position = Vector2.Lerp(startPos,endPos,movePercentage);
        }
        else
        {
            transform.position = Vector2.Lerp(endPos, startPos, movePercentage);
        }

        if(movePercentage >= 1.0f)
        {
            movePercentage = 0.0f; //進捗率をリセット
            isReverse = !isReverse; //反転フラグを逆転
            isCanMove = false; //とりあえず停める
            Invoke("Move", wait); //時間差で動き出すメソッド発動
        }

    }

    public void Move()
    {
        isCanMove = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Playerの親を自分に指定
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Playerの親をなくす＝解放
            collision.transform.SetParent(null);
        }
    }

    //移動範囲表示
    void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        //スプライトのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        //初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        //移動位置
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
