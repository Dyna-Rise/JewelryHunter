using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static string gameState; //ゲームの状態管理役 ※ 静的変数

    public GameObject stageTitle; //ステージタイトルのUIオブジェクト
    public Sprite gameClearSprite; //ゲームクリアの絵
    public Sprite gameOverSprite; //ゲームオーバーの絵


    public GameObject buttonPanel; //ボタンパネルのUIオブジェクト
    public GameObject restartButton; //リスタートボタン
    public GameObject nextButton; //ネクストボタン


    // Start is called before the first frame update
    void Start()
    {
        //ゲーム開始と同時にゲームステータスを"playing"
        gameState = "playing";

        Invoke("InactiveImage",1.0f); //第一引数に指定したメソッド(名)を、第二引数秒後に発動
        
        buttonPanel.SetActive(false); //オブジェクトを非表示

    }

    // Update is called once per frame
    void Update()
    {
        //ゲームの状態がクリアまたはオーバーの時、ボタンを復活させたい
        if(gameState == "gameclear" || gameState == "gameover")
        {
            //ステージタイトルを復活
            stageTitle.SetActive(true);

            //ボタンの復活
            buttonPanel.SetActive(true);
        }

        if(gameState == "gameclear")
        {
            stageTitle.GetComponent<Image>().sprite = gameClearSprite;

            //restartButtonオブジェクトがもっているButtonコンポーネントの値であるinteractableをfalse　→　ボタン機能を停止
            restartButton.GetComponent<Button>().interactable = false;
        }
        else if (gameState == "gameover")
        {
            stageTitle.GetComponent<Image>().sprite = gameOverSprite;

            //nextButtonオブジェクトがもっているButtonコンポーネントの値であるinteractableをfalse　→　ボタン機能を停止
            nextButton.GetComponent<Button>().interactable = false;
        }
    }

    //ステージタイトルを非表示にするメソッド
    void InactiveImage()
    {
        stageTitle.SetActive(false); //オブジェクトを非表示
    }
}
