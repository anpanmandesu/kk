using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    NavMeshAgent2D agent; //NavMeshAgent2D���g�p���邽�߂̕ϐ�
    [SerializeField] Transform target; //�ǐՂ���^�[�Q�b�g

    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>(); //agent��NavMeshAgent2D���擾
    }

    void Update()
    {
        agent.destination = target.position; //agent�̖ړI�n��target�̍��W�ɂ���
        //agent.SetDestination(target.position); //�������̏������ł��I�b�P�[
    }
}
