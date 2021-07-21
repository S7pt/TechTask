using UnityEngine;

public class CastleScript : MonoBehaviour
{
    [SerializeField] private Animator castleAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            castleAnimator.SetBool("IsClose",true);
        }
    }
}
