using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData) {
        
        CardController cardC = eventData.pointerDrag.GetComponent<CardController>();
        
        //  カードをプレイできるか確認しできないなら処理は無し
        bool canPlay = GameManager.instance.CanPlayCard(cardC);
        if (!canPlay) return;

        CardMovement cardM = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardM != null) {
            cardM.transform.SetParent(this.transform);
            cardM.cardParent = this.transform;  //  カードの親要素を自分(アタッチされているオブジェクト)に変更
        }
        
        //  プレイしたカードのスコアを計算し効果処理
        GameManager.instance.PlayCard(cardC);

        cardM.enabled = false;
    }
}
