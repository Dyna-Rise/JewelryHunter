using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    TimeController timeCnt; //TimeControllerスクリプトを取得する

    public TextMeshProUGUI timeText; //TimeTextオブジェクトが持っているTextMeshProコンポーネントの情報を取得

    public TextMeshProUGUI scoreText; //UIのテキスト
    public static int totalScore; //ゲームの合計得点
    public static int stageScore; //そのステージ中に手に入れたスコア

    AudioSource audio; //AudioSourceコンポーネントの情報取得
    public AudioClip meGameClear; //ゲームクリアの音源
    public AudioClip meGameOver; //ゲームオーバーの音源

    // Start is called before the first frame update
    void Start()
    {
        stageScore = 0; //ステージスコアをリセット

        //ゲーム開始と同時にゲームステータスを"playing"
        gameState = "playing";

        Invoke("InactiveImage",1.0f); //第一引数に指定したメソッド(名)を、第二引数秒後に発動
        
        buttonPanel.SetActive(false); //オブジェクトを非表示

        //TimeControllerコンポーネントの情報を取得
        timeCnt = GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateScore(); //スコア更新のメソッド

        if(gameState == "playing")
        {
            //カウントダウンの変数currentTimeをUIに連動
            //timeCntのcurrentTime(float型)をまずintに型変換してから、
            //ToString()で文字列に変換し、timeText(TextMeshPro)のtext欄に代入
            timeText.text = ((int)timeCnt.currentTime).ToString();

            //もしcurrentTimeが0になったらゲームオーバー
            if(timeCnt.currentTime <= 0)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player"); //プレイヤーを探す
                //探し出したプレイヤーが持っているPlayerControllerコンポーネントのGameOverメソッドを発動
                player.GetComponent<PlayerController>().GameOver();
            }

        }

        //ゲームの状態がクリアまたはオーバーの時、ボタンを復活させたい
        else if(gameState == "gameclear" || gameState == "gameover")
        {
            audio = GetComponent<AudioSource>();

            //ステージタイトルを復活
            stageTitle.SetActive(true);

            //ボタンの復活
            buttonPanel.SetActive(true);

            audio.Stop(); //曲を止める

            if (gameState == "gameclear")
            {
                stageTitle.GetComponent<Image>().sprite = gameClearSprite;

                //restartButtonオブジェクトがもっているButtonコンポーネントの値であるinteractableをfalse　→　ボタン機能を停止
                restartButton.GetComponent<Button>().interactable = false;

                totalScore += stageScore; //トータルスコアの更新
                stageScore = 0; //トータルに数字を渡せたので次に備えて0にリセット
                totalScore += (int)(timeCnt.currentTime * 10); //タイムボーナスをトータルにまぜて完了

               
                //audio.clip = meGameClear; //曲切り替え
                //audio.loop = false; //ループをオフ
                //audio.Play(); //曲再生

                audio.PlayOneShot(meGameClear); //1回だけ指定のサウンドを鳴らす
            }
            else if (gameState == "gameover")
            {
                stageTitle.GetComponent<Image>().sprite = gameOverSprite;

                //nextButtonオブジェクトがもっているButtonコンポーネントの値であるinteractableをfalse　→　ボタン機能を停止
                nextButton.GetComponent<Button>().interactable = false;

                //ゲームオーバー音を一度だけ鳴らす
                audio.PlayOneShot(meGameOver);
            }

            gameState = "gameend"; //重複して何回も同じ処理をしないように
        }
    }

    //ステージタイトルを非表示にするメソッド
    void InactiveImage()
    {
        stageTitle.SetActive(false); //オブジェクトを非表示
    }

    void UpdateScore()
    {
        //UIであるスコアテキスト(TextMeshProUGUIのテキスト欄）にトータルとステージの合計値を代入※文字列変換が必須
        scoreText.text = (stageScore + totalScore).ToString();
    }

}
