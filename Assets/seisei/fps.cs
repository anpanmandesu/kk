using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps : MonoBehaviour
{
    public float totalTimeInSeconds = 30f; // 5�� = 300�b
    public string agentTag = "Agent";
    public string FinishTag = "Finish";

    private float elapsedTime = 0f;
    [SerializeField]
    private int FPS = 60;
    public float time = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.1f; // ���s���Ԃ�1/10�ɂ���
        Application.targetFrameRate = FPS;
    }

    // Update is called once per frame
    void Update()
    {
        // �o�ߎ��Ԃ��X�V
        elapsedTime += Time.deltaTime;

        // �o�ߎ��Ԃ��w��̎��Ԃ𒴂�����I��
        if (elapsedTime >= totalTimeInSeconds)
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
        }

    }
}
