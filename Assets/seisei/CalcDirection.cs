using UnityEngine;

// �I�u�W�F�N�g�̌������擾����
public class CalcDirection : MonoBehaviour
{
    private Transform _transform;

    private void Start()
    {
        // transform�ɖ���A�N�Z�X����Əd���̂ŁA�L���b�V�����Ă���
        _transform = transform;
    }

    private void Update()
    {
        // ���ʁE�E�E�����
        var forward = _transform.forward;
        var right = _transform.right;
        var up = _transform.up;

        // ���E���E������
        var back = -forward;
        var left = -right;
        var down = -up;

        // ���O�\��
        //Debug.Log($"����:{forward}, �E:{right}, ��:{up}, ���{back}, ��{left}, ��{down}");
    }
}