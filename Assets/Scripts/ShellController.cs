using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour
{
    public float deleteTime = 3.0f; //削除されるまでの時間設定
    public bool isDelete; //ぶつかったら消える弾にしておくかどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        //時間差で消滅
        Destroy(gameObject, deleteTime); //deleteTimeだけ後に消滅
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //フラグが立っていれば消える
        if (isDelete)
        {
            Destroy(gameObject);
        }
    }

}
