using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardModel
{
    public int cardID;
    public string name;
    public bool TN;
    public CardEntity.CATEGORY category;
    public int Sat;     //  満足度
    public int Cos;     //  コスト
    public int Fat;     //  疲労度
    public int Tas;     //  タスク
    public int Com;     //  コミュ
    public int draw;    //  ドロー
    public int trash;   //  トラッシュ
    public Sprite icon;

    public CardModel(int arg_cardID) {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card" + arg_cardID);

        cardID = cardEntity.cardID;
        name = cardEntity.name;
        category = cardEntity.category;
        TN = cardEntity.TN;
        Sat = cardEntity.Sat;
        Cos = cardEntity.Cos;
        Fat = cardEntity.Fat;
        Tas = cardEntity.Tas;
        Com = cardEntity.Com;
        draw = cardEntity.draw;
        trash = cardEntity.trash;
        icon = cardEntity.icon;
    }
}
