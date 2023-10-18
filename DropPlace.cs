using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPlace : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData) {
        
        CardController cardC = eventData.pointerDrag.GetComponent<CardController>();
        
        //  �J�[�h���v���C�ł��邩�m�F���ł��Ȃ��Ȃ珈���͖���
        bool canPlay = GameManager.instance.CanPlayCard(cardC);
        if (!canPlay) return;

        CardMovement cardM = eventData.pointerDrag.GetComponent<CardMovement>();
        if (cardM != null) {
            cardM.transform.SetParent(this.transform);
            cardM.cardParent = this.transform;  //  �J�[�h�̐e�v�f������(�A�^�b�`����Ă���I�u�W�F�N�g)�ɕύX
        }
        
        //  �v���C�����J�[�h�̃X�R�A���v�Z�����ʏ���
        GameManager.instance.PlayCard(cardC);

        cardM.enabled = false;
    }
}
