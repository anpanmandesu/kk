using UnityEngine;

public class Containtest : MonoBehaviour
{

    private void Start()
    {
        string str = "�ڂ̑O�ɃX���C�������ꂽ�B";
        string target = "�X���C��";

        if (str.Contains(target))
        {
            Debug.Log($"{target}�����͂̒��Ɋ܂܂�Ă��܂����B");
        }
        else
        {
            Debug.Log($"{target}�͕��͂̒��Ɋ܂܂�Ă��܂���B");
        }
    }

}