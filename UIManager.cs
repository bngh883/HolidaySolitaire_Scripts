using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject NextTurnPanel;
    [SerializeField] TextMeshProUGUI NextTurnText;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] GameObject PlayFailedPanel;

    //  カードの詳細表示用
    [SerializeField] Transform Hand;
    [SerializeField] GameObject DetailPanel;
    [SerializeField] TextMeshProUGUI Name, category, sat, cos, fat, tas, com, draw, trash, effect;

    //  ターン開始時のパネル表示
    public IEnumerator ShowNextTurnPanel(int i) {
        NextTurnPanel.SetActive(true);
        yield return new WaitForSeconds(i);
        NextTurnPanel.SetActive(false);
    }

    //  使用不可のカードを使用したときのパネル表示
    public IEnumerator ShowPlayFailedPanel() {
        PlayFailedPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        PlayFailedPanel.SetActive(false);
    }

    public void ShowResultPanel() {
        ResultPanel.SetActive(true);
    }


    public void ShowDetailPanel(int i) {
        CardController card;
        
        //  カードのデータを取得
        CardController[] HandCardList = Hand.GetComponentsInChildren<CardController>();
        if (i < HandCardList.Length) {
            card = HandCardList[i];
        }
        else return;

        //  カードのデータをDetailPanelに表示
        sat.text = card.model.Sat.ToString();
        cos.text = card.model.Cos.ToString();
        fat.text = card.model.Fat.ToString();
        tas.text = card.model.Tas.ToString();
        com.text = card.model.Com.ToString();
        draw.text = card.model.draw.ToString();
        trash.text = card.model.trash.ToString();
        Name.text = "カード名\n「" + card.model.name + "」";
        switch (card.model.category) {
            case CardEntity.CATEGORY.Food:
                category.text = "カテゴリー\n｛めし｝";
                break;
            case CardEntity.CATEGORY.General:
                category.text = "カテゴリー\n｛はんよう｝";
                break;
            case CardEntity.CATEGORY.InHome:
                category.text = "カテゴリー\n｛ざいたく｝";
                break;
            case CardEntity.CATEGORY.OutDoor:
                category.text = "カテゴリー\n｛がいしゅつ｝";
                break;
            case CardEntity.CATEGORY.Special:
                category.text = "カテゴリー\n｛スペシャル｝";
                break;
            default:
                Debug.Log("そんざいしないカテゴリです");
                break;
        }


        DetailPanel.SetActive(true);
    }

    public void CloseDetailPanel() {

        DetailPanel.SetActive(false);
    }
}
