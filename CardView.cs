using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText, TNText, drawText, trashText;
    [SerializeField] Image iconImage;

    //  cardModelのデータ取得と反映
    public void Show(CardModel cardModel) {
        nameText.text = cardModel.name;
        if (cardModel.TN) {
            TNText.text = "T";
        }
        else {
            TNText.text = "N";
        }
        drawText.text = "+" + cardModel.draw.ToString();
        trashText.text = "-" + cardModel.trash.ToString();
        iconImage.sprite = cardModel.icon;

        //  カテゴリによって色を変更
        switch (cardModel.category) {
            case CardEntity.CATEGORY.Food:
                GetComponentInChildren<Image>().color = Color.green;
                break;
            case CardEntity.CATEGORY.OutDoor:
                GetComponentInChildren<Image>().color = Color.white;
                break;
            case CardEntity.CATEGORY.InHome:
                GetComponentInChildren<Image>().color = Color.cyan;
                break;
            case CardEntity.CATEGORY.Special:
                GetComponentInChildren<Image>().color = Color.gray;
                break;
            default:
                break;
        }

        //  変動しないスコアのアイコンを削除
        GameObject icons = transform.Find("Icons").gameObject;
        if (cardModel.Com == 0) icons.transform.GetChild(0).gameObject.SetActive(false);
        if (cardModel.Tas == 0) icons.transform.GetChild(1).gameObject.SetActive(false);
        if (cardModel.Fat == 0) icons.transform.GetChild(2).gameObject.SetActive(false);
        if (cardModel.Cos == 0) icons.transform.GetChild(3).gameObject.SetActive(false);
        if (cardModel.Sat == 0) icons.transform.GetChild(4).gameObject.SetActive(false);

    }
}
