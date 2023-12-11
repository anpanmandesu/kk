using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    public float totalTimeInSeconds = 30f; // 5�� = 300�b
    public string agentTag = "Agent";
    public string FinishTag = "Finish";
    public string FinishTag2 = "Finish2";

    private float elapsedTime = 0.1f;
    [SerializeField]
    private int FPS = 60;
    public float time = 0f;
    public float interval = 10;
    private int i = 1;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = FPS;
        // 30�b���Ƃ�CheckElapsedTime���\�b�h�����s
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            // �o�ߎ��Ԃ��X�V
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= interval && i != 10)
            {
                Debug.Log(i * 30 + "�b");
                // �^�O���t���Ă���Q�[���I�u�W�F�N�g�̔z����擾
                GameObject[] agents = GameObject.FindGameObjectsWithTag(agentTag);
                GameObject[] finishs = GameObject.FindGameObjectsWithTag(FinishTag);
                GameObject[] finishs2 = GameObject.FindGameObjectsWithTag(FinishTag2);
                // �z��̗v�f���i�Q�[���I�u�W�F�N�g�̐��j��\��
                Debug.Log($"Number of agents with tag '{agentTag}': {agents.Length}");
                Debug.Log($"Number of agents with tag '{FinishTag}': {finishs.Length + finishs2.Length}");
                elapsedTime = 0;
                i++;
            }

            // �o�ߎ��Ԃ��w��̎��Ԃ𒴂�����I��
            if (i == 10)
            {
                // �Q�[���I���̏����������ɋL�q
                Debug.Log("5���o�߂��܂����B�Q�[�����I�����܂��B");
                // �^�O��

                // �^�O���t���Ă���Q�[���I�u�W�F�N�g�̔z����擾
                GameObject[] agents = GameObject.FindGameObjectsWithTag(agentTag);
                GameObject[] finishs = GameObject.FindGameObjectsWithTag(FinishTag);

                // �z��̗v�f���i�Q�[���I�u�W�F�N�g�̐��j��\��
                Debug.Log($"Number of agents with tag '{agentTag}': {agents.Length}");
                Debug.Log($"Number of agents with tag '{FinishTag}': {finishs.Length}");
                // �A�v���P�[�V�������I��
                Time.timeScale = time;
                i++;
            }
        }

    }
    void Ac()
    {
        active = true;
    }
}
