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

    //  �J�[�h�̏ڍו\���p
    [SerializeField] Transform Hand;
    [SerializeField] GameObject DetailPanel;
    [SerializeField] TextMeshProUGUI Name, category, sat, cos, fat, tas, com, draw, trash, effect;

    //  �^�[���J�n���̃p�l���\��
    public IEnumerator ShowNextTurnPanel(int i) {
        NextTurnPanel.SetActive(true);
        yield return new WaitForSeconds(i);
        NextTurnPanel.SetActive(false);
    }

    //  �g�p�s�̃J�[�h���g�p�����Ƃ��̃p�l���\��
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
        
        //  �J�[�h�̃f�[�^���擾
        CardController[] HandCardList = Hand.GetComponentsInChildren<CardController>();
        if (i < HandCardList.Length) {
            card = HandCardList[i];
        }
        else return;

        //  �J�[�h�̃f�[�^��DetailPanel�ɕ\��
        sat.text = card.model.Sat.ToString();
        cos.text = card.model.Cos.ToString();
        fat.text = card.model.Fat.ToString();
        tas.text = card.model.Tas.ToString();
        com.text = card.model.Com.ToString();
        draw.text = card.model.draw.ToString();
        trash.text = card.model.trash.ToString();
        Name.text = "�J�[�h��\n�u" + card.model.name + "�v";
        switch (card.model.category) {
            case CardEntity.CATEGORY.Food:
                category.text = "�J�e�S���[\n�o�߂��p";
                break;
            case CardEntity.CATEGORY.General:
                category.text = "�J�e�S���[\n�o�͂�悤�p";
                break;
            case CardEntity.CATEGORY.InHome:
                category.text = "�J�e�S���[\n�o���������p";
                break;
            case CardEntity.CATEGORY.OutDoor:
                category.text = "�J�e�S���[\n�o��������p";
                break;
            case CardEntity.CATEGORY.Special:
                category.text = "�J�e�S���[\n�o�X�y�V�����p";
                break;
            default:
                Debug.Log("���񂴂����Ȃ��J�e�S���ł�");
                break;
        }


        DetailPanel.SetActive(true);
    }

    public void CloseDetailPanel() {

        DetailPanel.SetActive(false);
    }
}
