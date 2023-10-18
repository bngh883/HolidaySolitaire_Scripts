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

    [SerializeField] CardController cardPrefab;         //  カードのひな型
    [SerializeField] Transform Hand, Field;             //  盤面

    //  各数値関連（左上）
    [SerializeField] TextMeshProUGUI SatText, CosText, FatText, TasText, ComText;
    int SatTotal, CosTotal, FatTotal, TasTotal, ComTotal = 0;
     

    //  時間関連（右上）
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] GameObject TimeImages, BackImages;
    int t_index = 3;    //  [10時, 13時,  16時, 19時, 22時, 25時]
    int d_index = 2;    //  [日中, 夕方, 夜, 深夜]
    enum DAY {
        Fri,
        Sat,
        Sun
    }
    DAY daydata = DAY.Fri;

    //  飯関連（右下）
    [SerializeField] Image FoodIcon;    //  IsHungryがtrueの時だけ表示
    public bool IsHungry = true;

    //  ターミナルアクション制御
    public bool HasPlayedTerminal = false;
    //  プレイ失敗時のテキスト
    [SerializeField] TextMeshProUGUI PlayFailedText;
    //  ターン間ごとに表示するテキスト
    [SerializeField] TextMeshProUGUI NextTurnText;
    //  途中経過のスコア
    [SerializeField] TextMeshProUGUI ScoreText, RankText;
    int[] BorderList = { 2000000, 2500000, 3000000, 3500000, 4000000, 4700000, 100000000 };
    string[] RankList = {"E", "D" , "C", "B", "A", "S", "神", "神"};
    int rank_i = 0;

    //  リザルト
    long ResultScoreNum;
    [SerializeField] TextMeshProUGUI ResultScoreText, ResultMeal, ResultTask, ResultPlace, ResultComment;
    List<string> meal = new List<string>();
    List<string> task = new List<string>();
    List<string> place = new List<string>();
    

    List<int> deck = new List<int>() { 1, 2, 3, 5, 6, 7, 8, 14, 14, 14, 14, 10, 12, 12, 13, 13, 13, 20, 21, 22, 34, 24, 25, 26, 30, 31, 33, 35, 35, 36 };

    //  テスト用リスト
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

    //  ゲームスタート
    void StartGame() {
        deck = deck.OrderBy(a => Guid.NewGuid()).ToList();
        //  初期手札
        DrawCard(StartHand);
        NextTurnText.text = "有意義な休日をかけた戦いが今、始まる...！";
        StartCoroutine(uIManager.ShowNextTurnPanel(3));
    }

    //  カードIDを受け取って盤面に生成する関数
    public void CreateCard(int cardID, Transform place) {
        CardController card = Instantiate(cardPrefab, place);
        card.Init(cardID);
    }

    //  カードを引く
    void DrawCard(int num) {

        for (int i = 0; i < num; i++) {
            //  デッキが無いなら新しく生成
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

    //  カードをプレイする
    public void PlayCard(CardController card) {

        //  変動するスコア
        int sat = card.model.Sat;
        int cos = card.model.Cos;
        int fat = card.model.Fat;
        int tas = card.model.Tas;
        int com = card.model.Com;
        
        //  トラッシュ処理
        TrashCard(card.model.trash); 
        
        //  ドロー処理
        DrawCard(card.model.draw);

        if (card.model.TN) {
            HasPlayedTerminal = true;
        }

        //  めし共通効果
        if (card.model.category == CardEntity.CATEGORY.Food) {
            //  空腹でないときに飯を食っても満足度1/5
            if (!IsHungry) {
                sat /= 10;
            }
            IsHungry = false;
            FoodIcon.enabled = false;

            //  食ったものをリストに追加
            meal.Add(card.model.name);
        }
        else if (card.model.category == CardEntity.CATEGORY.OutDoor) {
            //  行った場所をリストに追加
            place.Add(card.model.name);
        }
        //  タスクをリストに追加
        if (card.model.Tas > 0) {
            task.Add(card.model.name);
        }
        
        //  合計スコアを加算
        SatTotal += sat;
        CosTotal += cos;
        FatTotal += fat;
        TasTotal += tas;
        ComTotal += com;

        //  計算後のスコアを表示
        SatText.SetText("{}", SatTotal);
        CosText.SetText("{}", CosTotal);
        FatText.SetText("{}", FatTotal);
        TasText.SetText("{}", TasTotal);
        ComText.SetText("{}", ComTotal);

        //  特殊効果
        PlayCardEffect(card.model.cardID);

        //  暫定スコアを更新
        UpdateScore(CalcScore());
    }

    //  カードをプレイできるかどうか判定
    public bool CanPlayCard(CardController card) {
        //  Tアクションがプレイ済みなら失敗
        if (HasPlayedTerminal) {
            PlayFailedText.text = "Tアクションはすでにつかっている！";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }
        //  空腹中に飯以外のターミナルアクションは失敗
        if (IsHungry &&
                card.model.category != CardEntity.CATEGORY.Food &&
                    card.model.TN) {
            PlayFailedText.text = "くうふくなのでまずはなにかたべないと!";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }
        //  ハンドの枚数よりトラッシュ枚数がおおいなら失敗
        if (Hand.childCount < card.model.trash) {
            PlayFailedText.text = "すてられるカードがない！";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }

        //  22時以降は外出カテゴリは失敗
        if (t_index > 3 &&
                card.model.category == CardEntity.CATEGORY.OutDoor) {
            PlayFailedText.text = "深夜にでかけるのはやめておこう！";
            StartCoroutine(uIManager.ShowPlayFailedPanel());
            return false;
        }

        return true;
    }

    //  ターン終了処理（ターン終了ボタンにアサイン）
    public void TurnEnd() {
        //  手札を全て捨てる
        TrashCard(Hand.childCount);

        //  盤面のカードを全て捨てる
        CardController[] FieldCardList = Field.GetComponentsInChildren<CardController>();
        foreach (CardController FieldCard in FieldCardList) {
            Destroy(FieldCard.gameObject);
        }

        HasPlayedTerminal = false;

        //  最終ターンが終わったならリザルトを表示
        if (t_index == 4 && daydata == DAY.Sun) {
            TimeImages.transform.GetChild(t_index).gameObject.SetActive(false);
            BackImages.transform.GetChild(d_index).gameObject.SetActive(false);
            dayText.text = "Mon";
            TimeImages.transform.GetChild(0).gameObject.SetActive(true);
            BackImages.transform.GetChild(0).gameObject.SetActive(true);

            //  計算したスコアを表示
            ResultScoreNum = CalcScore();
            ResultScoreText.text = ResultScoreNum.ToString();  

            //  リザルトコメント生成
            MakeResultComment();

            uIManager.ShowResultPanel();
            return;
        }

        //  手札を引き直す
        DrawCard(StartHand);

        UpdateTime();
        StartCoroutine(uIManager.ShowNextTurnPanel(1));
    }

    //  リトライ処理（リトライボタンにアサイン）
    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //  タイトルに戻る処理(タイトルボタンにアサイン)
    public void Title() {
        SceneManager.LoadScene("TitleScene");
    }

    //  途中スコアを更新(右下)
    void UpdateScore(long score) {
        //  ランクを更新
        while(score <= BorderList[6]  && score > BorderList[rank_i]) {
            rank_i++;
        }
        //  神到達時
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

    //  右上の日時を更新
    void UpdateTime() {
        TimeImages.transform.GetChild(t_index).gameObject.SetActive(false);
        BackImages.transform.GetChild(d_index).gameObject.SetActive(false);

        switch (t_index) {
            case 0:         //  10時から13時
                IsHungry = true;
                FoodIcon.enabled = true;
                t_index++;
                NextTurnText.text = "つぎはなにする？";
                break;
            case 1:         //  13時から16時
            case 4:         //  22時から25時
                t_index++; d_index++;
                break;
            case 2:         //  16時から19時
                IsHungry = true;
                FoodIcon.enabled = true;
                t_index++; d_index++;
                break;
            case 3:         //  19時から22時
                NextTurnText.text = "つぎはなにする？";
                t_index++;
                break;
            case 5:         //  翌日へ
                t_index = 0;
                d_index = 0;
                if (daydata == DAY.Fri) {
                    daydata = DAY.Sat;
                    NextTurnText.text = "どようびだ！";
                }
                else if (daydata == DAY.Sat) {
                    daydata = DAY.Sun;
                    NextTurnText.text = "もうにちようだと…。";
                }
                break;
            default:
                Debug.Log("時間のインデックス値に異常が発生しています。");
                break;
        }

        TimeImages.transform.GetChild(t_index).gameObject.SetActive(true);
        BackImages.transform.GetChild(d_index).gameObject.SetActive(true);
        dayText.text = daydata.ToString();
    }

    void MakeResultComment() {
        string meals = string.Join("\n", meal);
        ResultMeal.text = "食ったもの\n" + meals;
        string tasks = string.Join("\n", task);
        ResultTask.text = "稼いだアド\n" + tasks;
        string places = string.Join("\n", place);
        ResultPlace.text = "行った場所\n" + places;
        switch (rank_i) {
            case 0:
                ResultComment.text = "総評\n\nねてたらおわった。";
                break;
            case 1:
                ResultComment.text = "総評\n\nぐうたらしすぎたかなあ";
                break;
            case 2:
                ResultComment.text = "総評\n\nちょっともったいなかったかも";
                break;
            case 3:
                ResultComment.text = "総評\n\nそこそこリフレッシュできたね";
                break;
            case 4: 
                ResultComment.text = "総評\n\n充実した休みだったよ～";
                break;
            case 5:
                ResultComment.text = "総評\n\n最高の休日で気分もハッピー！";
                break;
            case 6:
                ResultComment.text = "総評\n\n休日の神、ヤスミンティウス降臨。";
                break;
            default:
                Debug.Log("そんざいしないランク");
                break;
        }
    }

    //  現在のスコアを計算
    long CalcScore() {
        long ret;
        ret = SatTotal * (30000 - CosTotal) / (100 + FatTotal / 30);
        if (TasTotal < 5) ret = ret / 10 *8;
        if (CosTotal < 100) ret = ret / 10 * 8;
        return ret;
    }


    //  カードの効果処理
    void PlayCardEffect(int cardID) {
        switch (cardID) {
            default:
                break;
        }
    }
}
