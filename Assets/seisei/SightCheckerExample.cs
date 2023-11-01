using UnityEngine;

public class SightCheckerExample : MonoBehaviour
{

    // �^�[�Q�b�g
    [SerializeField] private Transform _target;

    // ����p�i�x���@�j
    [SerializeField] private float _sightAngle;

    // ���E�̍ő勗��
    [SerializeField] private float _maxDistance = float.PositiveInfinity;

    #region Logic

    /// <summary>
    /// �^�[�Q�b�g�������Ă��邩�ǂ���
    /// </summary>
    public bool IsVisible()
    {
        // ���g�̈ʒu
        var selfPos = this.transform.position;
        // �^�[�Q�b�g�̈ʒu
        var targetPos = _target.position;

        // ���g�̌����i���K�����ꂽ�x�N�g���j
        var selfDir = this.transform.forward;
        
        // �^�[�Q�b�g�܂ł̌����Ƌ����v�Z
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(��/2)���v�Z
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z
        // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // ���E����
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    #endregion

    #region Debug

    // ���E����̌��ʂ�GUI�o��
    private void OnGUI()
    {
        // ���E����
        var isVisible = IsVisible();

        // ���ʕ\��
        GUI.Box(new Rect(20, 20, 150, 23), $"isVisible = {isVisible}");
    }

    #endregion
}