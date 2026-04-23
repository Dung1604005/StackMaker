using UnityEngine;

public class WinArea : MonoBehaviour
{
    [SerializeField] private ParticleSystem win1Particle;

    [SerializeField] private ParticleSystem win2Particle;

    [SerializeField] private GameObject chessClose;

    [SerializeField] private GameObject chessOpen;

    public void OnInit()
    {
        win1Particle.Pause();
        win2Particle.Pause();
        chessClose.SetActive(true);
        chessOpen.SetActive(false);
    }

    public void TriggerWin()
    {
        win1Particle.Play();
        win2Particle.Play();
        chessClose.SetActive(false);

        chessOpen.SetActive(true);

        EventBus<OnWinEvent>.Raise(new OnWinEvent
        {
            
        });
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameConfig.PLAYER_TAG))
        {
            TriggerWin();
        }
    }





}
