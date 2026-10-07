using UnityEngine;
using UnityEngine.SceneManagement; //シーンの切替に必要
using TMPro; //TextMeshProを使うのに必要
public class ResultManager : MonoBehaviour
{
    //パターン1
    public TextMeshProUGUI scoreText; //コンポーネントから行きたい場合

    //パターン2
    public GameObject scoreTextObject; //ゲームオブジェクトからいきたい場合

    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //パターン1 コンポーネントからtextを指定 ※こちらが早い
        scoreText.text = GameManager.totalScore.ToString();
        //パターン2 ゲームオブジェクトからtextを指定
        scoreTextObject.GetComponent<TextMeshProUGUI>().text = GameManager.totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //シーンを読み込む
    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
