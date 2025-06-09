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

    AudioSource audio;
    public AudioClip jumpSE;

    bool isMobileInput; //モバイルの入力があったかどうか

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

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
        if (GameController.gameState != "playing")
        {
            return; //Updateの処理を強制終了
        }

        //左右のキーがおされたら、どっちの値だったのかをaxisHに格納
        //引数Horizontalの場合：水平方向のキーが何か押された場合
        //左なら-1、右なら1、何も押されてないのであれば常に0を返すメソッド

        //モバイル端末からの入力を検知してフラグが立っている間はキーボード情報は採用されない
        if (!isMobileInput)
        {
            axisH = Input.GetAxisRaw("Horizontal");
        }

        //もしaxisHが正の数なら右向き
        if (axisH > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            animator.SetBool("run", true); //担当しているコントローラーのパラメータを変える
        }
        //でなければもしaxisHが負の数なら左向き
        else if (axisH < 0)
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
        if(GameController.gameState != "playing")
        {
            return;
        }

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
            rbody.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
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
            audio.PlayOneShot(jumpSE);
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
            Goal();
        }

        if (collision.gameObject.tag == "Dead")
        {
            GameOver();
        }

        if (collision.gameObject.tag == "Item") //Itemに触れたら
        {
            ItemData itemdata = collision.gameObject.GetComponent<ItemData>(); //ぶつかったItemのスクリプトを取得
            GameController.stageScore += itemdata.value; //ぶつかったItemのスクリプトに記されているvalueの値をstageScoreに加算
            Destroy(collision.gameObject); //用が済んだので相手の本体を抹消
        }
    }

    public void Goal()
    {
        GameController.gameState = "gameclear";
        animator.SetBool("gameClear", true); //PlayerClearアニメをON
        PlayerStop(); //動きを止める
    }

    public void GameOver()
    {
        GameController.gameState = "gameover";
        animator.SetBool("gameOver", true);
        PlayerStop(); //動きを止める

        //プレイヤーを上に跳ね上げる
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        //当たり判定もカット
        GetComponent<CapsuleCollider2D>().enabled = false;

    }

    //プレイヤーの動きを停止
    public void PlayerStop()
    {
        //速度を0にして止める
        rbody.velocity = new Vector2(0, 0);
    }

    public void MobileAxis(float axis)
    {
        axisH = axis; //MobileStick.cs経由で与えられた引数axisの値が入る（1か-1か)

        //axisに値が入っていたということであれば
        //モバイルUIが触られたということになるため
        //モバイル入力フラグをONにする
        if (axisH == 0) isMobileInput = false;
        else isMobileInput = true;
    }

}
