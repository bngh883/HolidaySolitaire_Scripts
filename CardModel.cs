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
    public int Sat;     //  �����x
    public int Cos;     //  �R�X�g
    public int Fat;     //  ��J�x
    public int Tas;     //  �^�X�N
    public int Com;     //  �R�~��
    public int draw;    //  �h���[
    public int trash;   //  �g���b�V��
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
