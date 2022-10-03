using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    AudioClip _select;

    IAudioController _audio;

    [Inject]
    void Inject(IAudioController audio)
    {
        _audio = audio;
    }

    void Start()
    {
        InputSystem.onAnyButtonPress
            .Subscribe(_ =>
            {
                _audio.Play(_select, 1, 1);
                SceneManager.LoadScene("SampleScene");
            })
            .AddTo(this);
    }
}
