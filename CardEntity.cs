using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardEntity", menuName = "Create CardEntity")]

public class CardEntity : ScriptableObject
{
    public enum CATEGORY {
        Food,
        OutDoor,
        InHome,
        General,
        Special
    }

    /***    ID：名前
     * 
     * 外出
     *       1：映画
     *       2：水族館
     *       3：公園
     *       4：動物園
     *       5：スーパー
     *       6：銭湯
     *       7：オタク
     *       8：本屋
     *       
     * 汎用
     *      10：読書
     *      11：漫画
     *      12：Youtube
     *      13：一息つく
     *      14：Twitter
     *      
     * 食事
     *      20：ラーメン
     *      21：ハンバーガー
     *      22：牛丼
     *      23：酒とつまみ
     *      24：パスタ
     *      25：とんかつ
     *      26：カレー
     *   
     * 自宅
     *      30：勉強
     *      31：洗濯
     *      32：昼寝
     *      33：通話
     *      34：ゲーム
     *      35：テレビ
     *      36：掃除
     * 
     */
    public int cardID;
    public new string name;
    public CATEGORY category;
    public bool TN;
    public int Sat;     //  満足度
    public int Cos;     //  コスト
    public int Fat;     //  疲労度
    public int Tas;     //  タスク
    public int Com;     //  コミュ
    public int draw;    //  ドロー
    public int trash;   //  トラッシュ
    public Sprite icon; 
    
}
