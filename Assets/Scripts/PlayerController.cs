using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PlayerController : MonoBehaviour
{
    private float axisH; //左右のキーの値を格納
    Rigidbody2D rbody; //Rigidbody2Dの情報を扱う為の媒体
    Animator animator; //Animatorの情報を扱う為の媒体

    public float speed = 3.0f; //歩くスピード 
    bool isJump; //ジャンプ中かどうか
    bool onGround; //地面判定
    public LayerMask groundLayer; //地面判定の対象のレイヤーが何かを決めておく
    public float jump = 9.0f; //ジャンプ力

    // Start is called before the first frame update
    void Start()
    {
        //PlayerについているRigidbody2Dコンポーネントを
        //変数rbodyに宿した。以後、Rigidbody2Dコンポーネントの
        //機能はrbodyという変数を通してプログラム側から活用できる
        rbody = GetComponent<Rigidbody2D>();

        //PlayerについているAnimatorコンポーネントを変数animatorに宿した
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //左右のキーがおされたら、どっちの値だったのかをaxisHに格納
        //引数Horizontalの場合：水平方向のキーが何か押された場合
        //左なら-1、右なら1、何も押されてないのであれば常に0を返すメソッド
        axisH = Input.GetAxisRaw("Horizontal");

        //もしaxisHが正の数なら右向き
        if(axisH > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("Run",true); //担当しているコントローラーのパラメータを変える
        }
        //でなければもしaxisHが負の数なら左向き
        else if(axisH < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            animator.SetBool("run", true); //担当しているコントローラーのパラメータを変える
        }
        else
        {
            animator.SetBool("run", false); //担当しているコントローラーのパラメータを変える
        }


        //もしもジャンプボタンがおされたら
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void FixedUpdate()
    {
        //地面にいるかどうかをサークルキャストを使って判別
        onGround = Physics2D.CircleCast(
                transform.position,　//Playerの基準点
                0.2f,//半径
                Vector2.down, //指定した点からどの方向にチェックを伸ばすか new Vector2(0,-1)
                0.0f, //指定した点からどのくらいチェックの距離を伸ばすか
                groundLayer　//指定したレイヤー
            );

        //velocityに2軸の方向データ(Vector2)を代入
        rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);

        //ジャンプ中フラグが立ったら
        if (isJump)
        {
            //Rigidbody2DのAddForceメソッドによって上に押し出す
            rbody.AddForce(new Vector2(0,jump),ForceMode2D.Impulse);
            isJump = false;
        }
    }

    public void Jump()
    {
        //地面判定がfalseならジャンプフラグは立てない
        if (onGround)
        {
            isJump = true;
            animator.SetTrigger("jump"); //ジャンプアニメのためのトリガー発動
        }
    }

    //何かとぶつかったら発動するメソッド
    //ぶつかった相手のCollider情報を引数collisionに入れる
    //相手にColliderがついていないと意味がない
    //※相手のColliderがisTriggerであること
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            GameController.gameState = "gameclear";
        }

        if (collision.gameObject.tag == "Dead")
        {
            GameController.gameState = "gameover";
        }
    }

}
