using UnityEngine;
using UnityEngine.UI;               // UIを使うのに必要
using UnityEngine.SceneManagement; //シーン切替に必要なクラスがある

public class GameManager : MonoBehaviour
{
    public GameObject mainImage; //画像を持つImageゲームオブジェクト
    public Sprite gameOverSpr; //GAMEOVER画像
    public Sprite gameClearSpr; //GAMECLER画像
    public GameObject panel; //パネル
    public GameObject restartButton; //RESTARTボタン
    public GameObject nextButton; //NEXTボタン

    Image titleImage; //画像を表示するImageコンポーネント
    GameState gamestate = GameState.InGame; //ゲームの状態

    public string nextSceneName; //次のシーン名

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //テキストをみて完成させましょう
        Invoke("InactiveImage",1.0f); //1秒後にInactiveImageメソッドを発動
        panel.SetActive(false); //パネルを即非表示

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == GameState.GameClear)
        {
            //テキストをみて完成させましょう
            gamestate = GameState.GameClear;
            mainImage.SetActive(true); //画像表示
            panel.SetActive(true); //ボタンの表示
            //RESTARTボタン無効化
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false; //ボタン機能を無効化

            //メイン画像の差し替え
            mainImage.GetComponent<Image>().sprite = gameClearSpr;

            //titleImage = mainImage.GetComponent<Image>();
            //titleImage.sprite = gameClearSpr;

            PlayerController.gameState = GameState.GameEnd;
        }
        else if (PlayerController.gameState == GameState.GameOver)
        {
            //テキストをみて完成させましょう
            gamestate = GameState.GameOver;
            mainImage.SetActive(true); //画像表示
            panel.SetActive(true); //ボタンの表示
            //RESTARTボタン無効化
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false; //ボタン機能を無効化

            //メイン画像の差し替え
            mainImage.GetComponent<Image>().sprite = gameOverSpr;

            PlayerController.gameState = GameState.GameEnd;
        }
        else if (PlayerController.gameState == GameState.InGame)
        {
            
        }
    }

    // 画像を非表示にする
    void InactiveImage()
    {
        //テキストではハイライトされていませんがここも編集！テキストをみて完成させましょう
        mainImage.SetActive(false); //オブジェクトを非表示
    }

    //リスタート
    public void Restart()
    {
        //現シーンの名前を引数に入れる
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //次へ
    public void Next()
    {
        SceneManager.LoadScene(nextSceneName);
    }

}
