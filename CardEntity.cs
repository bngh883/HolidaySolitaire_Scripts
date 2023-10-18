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

    /***    ID�F���O
     * 
     * �O�o
     *       1�F�f��
     *       2�F������
     *       3�F����
     *       4�F������
     *       5�F�X�[�p�[
     *       6�F�K��
     *       7�F�I�^�N
     *       8�F�{��
     *       
     * �ėp
     *      10�F�Ǐ�
     *      11�F����
     *      12�FYoutube
     *      13�F�ꑧ��
     *      14�FTwitter
     *      
     * �H��
     *      20�F���[����
     *      21�F�n���o�[�K�[
     *      22�F����
     *      23�F���Ƃ܂�
     *      24�F�p�X�^
     *      25�F�Ƃ񂩂�
     *      26�F�J���[
     *   
     * ����
     *      30�F�׋�
     *      31�F����
     *      32�F���Q
     *      33�F�ʘb
     *      34�F�Q�[��
     *      35�F�e���r
     *      36�F�|��
     * 
     */
    public int cardID;
    public new string name;
    public CATEGORY category;
    public bool TN;
    public int Sat;     //  �����x
    public int Cos;     //  �R�X�g
    public int Fat;     //  ��J�x
    public int Tas;     //  �^�X�N
    public int Com;     //  �R�~��
    public int draw;    //  �h���[
    public int trash;   //  �g���b�V��
    public Sprite icon; 
    
}
