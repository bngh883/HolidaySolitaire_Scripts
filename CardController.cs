using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    CardView view;       //  カードの見た目処理
    public CardModel model;     //  カードのデータ処理

    private void Awake() {
        view = GetComponent<CardView>();
    }

    //  カード生成関数
    public void Init(int cardID) {
        model = new CardModel(cardID);      //  カードデータ生成
        view.Show(model);                   //  表示
    }
}
