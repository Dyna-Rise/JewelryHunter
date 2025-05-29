using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float axisH; //左右のキーの値を格納
    Rigidbody2D rbody; //Rigidbody2Dの情報を扱う為の媒体
    public float speed = 3.0f; //歩くスピード 


    // Start is called before the first frame update
    void Start()
    {
        //PlayerについているRigidbody2Dコンポーネントを
        //変数rbodyに宿した。以後、Rigidbody2Dコンポーネントの
        //機能はrbodyという変数を通してプログラム側から活用できる
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //左右のキーがおされたら、どっちの値だったのかをaxisHに格納
        //引数Horizontalの場合：水平方向のキーが何か押された場合
        //左なら-1、右なら1、何も押されてないのであれば常に0を返すメソッド
        axisH = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        //velocityに2軸の方向データ(Vector2)を代入
        rbody.velocity = new Vector2(axisH * speed, rbody.velocity.y);
    }
}
