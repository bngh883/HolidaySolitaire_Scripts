using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
   //   �Q�[���X�^�[�g�iPlay�{�^���ɓK�p�j
   public void PlayStart() {
        SceneManager.LoadScene("MainScene");
    }
}
