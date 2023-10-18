using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    const int StartHand = 4;

    [SerializeField] UIManager uIManager;

    [SerializeField] CardController cardPrefab;         //  �J�[�h�̂ЂȌ^
    [SerializeField] Transform Hand, Field;             //  �Ֆ�

    //  �e���l�֘A�i����j
    [SerializeField] TextMeshProUGUI SatText, CosText, FatText, TasText, ComText;
    int SatTotal, CosTotal, FatTotal, TasTotal, ComTotal = 0;
     

    //  ���Ԋ֘A�i�E��j
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] GameObject TimeImages, BackImages;
    int t_index = 3;    //  [10��, 13��,  16��, 19��, 22��, 25��]
    int d_index = 2;    //  [����, �[��, ��, �[��]
    enum DAY {
        Fri,
        Sat,
        Sun
    }
    DAY daydata = DAY.Fri;

    //  �ъ֘A�i�E���j
    [SerializeField] Image FoodIcon;    //  IsHungry��true�̎������\��
    public bool IsHungry = true;

    //  �^�[�~�i���A�N�V��������
    public bool HasPlayedTerminal = false;
    //  �v���C���s���̃e�L�X�g
    [SerializeField] TextMeshProUGUI PlayFailedText;
    //  �^�[���Ԃ��Ƃɕ\������e�L�X�g
    [SerializeField] TextMeshProUGUI NextTurnText;
    //  �r���o�߂̃X�R�A
    [SerializeField] TextMeshProUGUI ScoreText, RankText;
    int[] BorderList = { 2000000, 2500000, 3000000, 3500000, 4000000, 4700000, 100000000 };
    string[] RankList = {"E", "D" , "C", "B", "A", "S", "�_", "�_"};
    int rank_i = 0;

    //  ���U���g
    long ResultScoreNum;
    [SerializeField] TextMeshProUGUI ResultScoreText, ResultMeal, ResultTask, ResultPlace, ResultComment;
    List<string> meal = new List<string>();
    List<string> task = new List<string>();
    List<string> place = new List<string>();
    

    List<int> deck = new List<int>() { 1, 2, 3, 5, 6, 7, 8, 14, 14, 14, 14, 10, 12, 12, 13, 13, 13, 20, 21, 22, 34, 24, 25, 26, 30, 31, 33, 35, 35, 36 };

    //  �e�X�g�p���X�g
    //  {1, 2, 3, 5, 6, 7, 8, 14, 14, 14, 14, 10, 12, 12, 13, 13, 13, 20, 21, 22, 23, 24, 25, 26, 30, 31, 33, 35, 35, 36 };
    //  {13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14, 13, 14};
    //  { 1, 2, 3, 4, 5, 6, 7, 8, 10, 11, 12, 13, 14, 20, 21, 22, 23, 24, 25, 26, 30, 31, 32, 33, 34, 35, 36 };

    public static GameManager instance;
    public void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start(){
        StartGame();
    }

    //  �Q�[���X�^�[�g
    void StartGame() {
        deck = deck.OrderBy(a => Guid.NewGuid()).ToList();
        //  ������D
        DrawCard(StartHand);
        NextTurnText.text = "�L�Ӌ`�ȋx�����������킢�����A�n�܂�...�I";
        StartCoroutine(uIManager.ShowNextTurnPanel(3));
    }

    //  �J�[�hID���󂯎���ĔՖʂɐ�������֐�
    public void CreateCard(int cardID, Transform place) {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }

    //  �J�[�h������
    void DrawCard(int num) {

        for (int i = 0; i < num; i++) {
            //  �f�b�L�������Ȃ�V��������
            if (deck.Count == 0) {
                deck = new List<int>() { 1, 2, 3, 5, 6, 7, 8, 14, 14, 14, 14, 10, 12, 12, 13, 13, 13, 20, 21, 22, 34, 24, 25, 26, 30, 31, 33, 35, 35, 36 };
                deck = deck.OrderBy(a => Guid.NewGuid()).ToList();
            }
        
            int cardID = deck[0];
            deck.RemoveAt(0);
            CreateCard(cardID, Hand);
        }
    }

    void TrashCard(int num) {
        CardController[] HandCardList = Hand.GetComponentsInChildren<CardController>();
        for (int i = 0; i < num; i++) {
            Destroy(HandCardList[i].gameObject);
        }
    }

    //  �J�[�h���v���C����
    public void PlayCard(CardController card) {

        //  �ϓ�����X�R�A
        int sat = card.model.Sat;
        int cos = card.model.Cos;
        int fat = card.model.Fat;
        int tas = card.model.Tas;
        int com = card.model.Com;
        
        //  �g���b�V������
        TrashCard(card.model.trash); 
        
        //  �h���[����
        DrawCard(card.model.draw);

        if (card.model.TN) {
            HasPlayedTerminal = true;
        }

        //  �߂����ʌ���
        if (card.model.category == CardEntity.CATEGORY.Food) {
            //  �󕠂łȂ��Ƃ��ɔт�H���Ă������x1/5
            if (!IsHungry) {
                sat /= 10;
            }
            IsHungry = false;
            FoodIcon.enabled = false;

            //  �H�������̂����X�g�ɒǉ�
            meal.Add(card.model.name);
        }
        else if (card.model.category == CardEntity.CATEGORY.OutDoor) {
            //  �s�����ꏊ�����X�g�ɒǉ�
            place.Add(card.model.name);
        }
        //  �^�X�N�����X�g�ɒǉ�
        if (card.model.Tas > 0) {
            task.Add(card.model.name);
        }
        
        //  ���v�X�R�A�����Z
        SatTotal += sat;
        CosTotal += cos;
        FatTotal += fat;
        TasTotal += tas;
        ComTotal += com;

        //  �v�Z��̃X�R�A��\��
        SatText.SetText("{}", SatTotal);
        CosText.SetText("{}", CosTotal);
        FatText.SetText("{}", FatTotal);
        TasText.SetText("{}", TasTotal);
        ComText.SetText("{}", ComTotal);

        //  �������
        PlayCardEffect(card.model.cardID);

        //  �b��X�R�A���X�V
        UpdateScore(CalcScore());
    }

    //  �J�[�h���v���C�ł��邩�ǂ�������
    public bool CanPlayCard(CardController card) {
        //  T�A�N�V�������v���C�ς݂Ȃ玸�s
        if (HasPlayedTerminal) {
            PlayFailedText.text = "T�A�N�V�����͂��łɂ����Ă���I";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }
        //  �󕠒��ɔшȊO�̃^�[�~�i���A�N�V�����͎��s
        if (IsHungry &&
                card.model.category != CardEntity.CATEGORY.Food &&
                    card.model.TN) {
            PlayFailedText.text = "�����ӂ��Ȃ̂ł܂��͂Ȃɂ����ׂȂ���!";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }
        //  �n���h�̖������g���b�V���������������Ȃ玸�s
        if (Hand.childCount < card.model.trash) {
            PlayFailedText.text = "���Ă���J�[�h���Ȃ��I";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }

        //  22���ȍ~�͊O�o�J�e�S���͎��s
        if (t_index > 3 &&
                card.model.category == CardEntity.CATEGORY.OutDoor) {
            PlayFailedText.text = "�[��ɂł�����̂͂�߂Ă������I";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }

        return true;
    }

    //  �^�[���I�������i�^�[���I���{�^���ɃA�T�C���j
    public void TurnEnd() {
        //  ��D��S�Ď̂Ă�
        TrashCard(Hand.childCount);

        //  �Ֆʂ̃J�[�h��S�Ď̂Ă�
        CardController[] FieldCardList = Field.GetComponentsInChildren<CardController>();
        foreach (CardController FieldCard in FieldCardList) {
            Destroy(FieldCard.gameObject);
        }

        HasPlayedTerminal = false;

        //  �ŏI�^�[�����I������Ȃ烊�U���g��\��
        if (t_index == 4 && daydata == DAY.Sun) {
            TimeImages.transform.GetChild(t_index).gameObject.SetActive(false);
            BackImages.transform.GetChild(d_index).gameObject.SetActive(false);
            dayText.text = "Mon";
            TimeImages.transform.GetChild(0).gameObject.SetActive(true);
            BackImages.transform.GetChild(0).gameObject.SetActive(true);

            //  �v�Z�����X�R�A��\��
            ResultScoreNum = CalcScore();
            ResultScoreText.text = ResultScoreNum.ToString();  

            //  ���U���g�R�����g����
            MakeResultComment();

            uIManager.ShowResultPanel();
            return;
        }

        //  ��D����������
        DrawCard(StartHand);

        UpdateTime();
        StartCoroutine(uIManager.ShowNextTurnPanel(1));
    }

    //  ���g���C�����i���g���C�{�^���ɃA�T�C���j
    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //  �^�C�g���ɖ߂鏈��(�^�C�g���{�^���ɃA�T�C��)
    public void Title() {
        SceneManager.LoadScene("TitleScene");
    }

    //  �r���X�R�A���X�V(�E��)
    void UpdateScore(long score) {
        //  �����N���X�V
        while(score <= BorderList[6]  && score > BorderList[rank_i]) {
            rank_i++;
        }
        //  �_���B��
        if (rank_i == 6) {
            long rest = BorderList[5] - score;
            ScoreText.text = rest.ToString();
        }
        else {
            long rest = BorderList[rank_i] - score;
            ScoreText.text = rest.ToString();
        }
        RankText.text = RankList[rank_i +1];
    }

    //  �E��̓������X�V
    void UpdateTime() {
        TimeImages.transform.GetChild(t_index).gameObject.SetActive(false);
        BackImages.transform.GetChild(d_index).gameObject.SetActive(false);

        switch (t_index) {
            case 0:         //  10������13��
                IsHungry = true;
                FoodIcon.enabled = true;
                t_index++;
                NextTurnText.text = "���͂Ȃɂ���H";
                break;
            case 1:         //  13������16��
            case 4:         //  22������25��
                t_index++; d_index++;
                break;
            case 2:         //  16������19��
                IsHungry = true;
                FoodIcon.enabled = true;
                t_index++; d_index++;
                break;
            case 3:         //  19������22��
                NextTurnText.text = "���͂Ȃɂ���H";
                t_index++;
                break;
            case 5:         //  ������
                t_index = 0;
                d_index = 0;
                if (daydata == DAY.Fri) {
                    daydata = DAY.Sat;
                    NextTurnText.text = "�ǂ悤�т��I";
                }
                else if (daydata == DAY.Sat) {
                    daydata = DAY.Sun;
                    NextTurnText.text = "�����ɂ��悤���Ɓc�B";
                }
                break;
            default:
                Debug.Log("���Ԃ̃C���f�b�N�X�l�Ɉُ킪�������Ă��܂��B");
                break;
        }

        TimeImages.transform.GetChild(t_index).gameObject.SetActive(true);
        BackImages.transform.GetChild(d_index).gameObject.SetActive(true);
        dayText.text = daydata.ToString();
    }

    void MakeResultComment() {
        string meals = string.Join("\n", meal);
        ResultMeal.text = "�H��������\n" + meals;
        string tasks = string.Join("\n", task);
        ResultTask.text = "�҂����A�h\n" + tasks;
        string places = string.Join("\n", place);
        ResultPlace.text = "�s�����ꏊ\n" + places;
        switch (rank_i) {
            case 0:
                ResultComment.text = "���]\n\n�˂Ă��炨������B";
                break;
            case 1:
                ResultComment.text = "���]\n\n�������炵���������Ȃ�";
                break;
            case 2:
                ResultComment.text = "���]\n\n������Ƃ��������Ȃ���������";
                break;
            case 3:
                ResultComment.text = "���]\n\n�����������t���b�V���ł�����";
                break;
            case 4: 
                ResultComment.text = "���]\n\n�[�������x�݂�������`";
                break;
            case 5:
                ResultComment.text = "���]\n\n�ō��̋x���ŋC�����n�b�s�[�I";
                break;
            case 6:
                ResultComment.text = "���]\n\n�x���̐_�A���X�~���e�B�E�X�~�ՁB";
                break;
            default:
                Debug.Log("���񂴂����Ȃ������N");
                break;
        }
    }

    //  ���݂̃X�R�A���v�Z
    long CalcScore() {
        long ret;
        ret = SatTotal * (30000 - CosTotal) / (100 + FatTotal / 30);
        if (TasTotal < 5) ret = ret / 10 *8;
        if (CosTotal < 100) ret = ret / 10 * 8;
        return ret;
    }


    //  �J�[�h�̌��ʏ���
    void PlayCardEffect(int cardID) {
        switch (cardID) {
            default:
                break;
        }
    }
}
