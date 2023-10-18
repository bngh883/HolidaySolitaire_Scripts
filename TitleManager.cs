using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
   //   ゲームスタート（Playボタンに適用）
   public void PlayStart() {
        SceneManager.LoadScene("MainScene");
    }
}
