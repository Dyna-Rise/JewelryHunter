using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f; //移動スピード
    public bool isToRight; //true = 右向き、 false = 左向き
    int groundContactCount; //地面にどのくらい接触したか

    public LayerMask groundLayer; //地面判定の対象レイヤー

    // Start is called before the first frame update
    void Start()
    {
        if (isToRight)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        bool onGround = Physics2D.CircleCast(
            transform.position, //センサーの発生位置
            0.5f, //円の半径
            Vector2.down, //どこの方角に向けるか
            0, //センサーを飛ばす距離
            groundLayer //調査対象のレイヤー
            );

        if (onGround)
        {
            Rigidbody2D rbody = GetComponent<Rigidbody2D>();
            if (isToRight)
            {
                rbody.velocity = new Vector2(speed, rbody.velocity.y);
            }
            else
            {
                rbody.velocity = new Vector2(-speed, rbody.velocity.y);
            }
        }
    }

    //左か右かの切り替えメソッド
    void Turn()
    {
        isToRight = !isToRight;

        if (isToRight) transform.localScale = new Vector2(-1, 1);
        else transform.localScale = new Vector2(1, 1);
    }

    //Groundタグ以外のものとぶつかったら反転
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Ground") { 
            Turn(); //何かとぶつかったら反転
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            groundContactCount++; //あたらしい地面と接触したら1カウントプラス
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            groundContactCount--; //何かの地面から脱出したら1カウントマイナス

            if (groundContactCount <= 0) //マイナスした結果、新たに接触する地面もない時はカウントは0→崖際にいる
            {
                groundContactCount = 0;　//念のためカウントを明確に0に戻す
                Turn(); //崖際だと思われるので反転
            }
        }
    }

}
