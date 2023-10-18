using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    CardView view;       //  �J�[�h�̌����ڏ���
    public CardModel model;     //  �J�[�h�̃f�[�^����

    private void Awake() {
        view = GetComponent<CardView>();
    }

    //  �J�[�h�����֐�
    public void Init(int cardID) {
        model = new CardModel(cardID);      //  �J�[�h�f�[�^����
        view.Show(model);                   //  �\��
    }
}
