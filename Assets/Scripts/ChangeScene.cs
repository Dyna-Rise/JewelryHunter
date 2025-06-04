using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //SceneManagerクラスを使うのに必要

public class ChangeScene : MonoBehaviour
{
    public string sceneName;

    public void Load()
    {
        //SceneManagerクラスがもっているメソッドを使ってシーン切り替え
        SceneManager.LoadScene(sceneName);
    }

}
