using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    GameObject _gameOverText;

    [SerializeField]
    TextMeshProUGUI _artifactsCollectedText;

    [SerializeField]
    GameObject _restartText;

    [SerializeField]
    TextMeshProUGUI _artifactText;

    [SerializeField]
    TextMeshProUGUI _timerText;

    [SerializeField]
    Image _healthFill;

    [SerializeField]
    TextMeshProUGUI _killsText;

    [SerializeField]
    GameObject _healthContainer;

    IMessageBroker _broker;
    IGameTimer _timer;

    [Inject]
    void Inject(IMessageBroker broker, IGameTimer timer)
    {
        _broker = broker;
        _timer = timer;
    }

    public void SetEnemiesKilled(int killed)
    {
        _killsText.text = killed.ToString();
    }

    public void ShowRestartText()
    {
        _restartText.SetActive(true);
    }

    public void ShowArtifactsCollectedText(List<ArtifactData> collected)
    {
        _artifactsCollectedText.text = $"Collected {collected.Count} Artifacts!";
        _artifactsCollectedText.enabled = true;
        _artifactsCollectedText.gameObject.SetActive(true);
    }

    public void ShowPickedUpItem(ArtifactData data)
    {
        _artifactText.text = $"Picked up {data.ArtifactName}!";
        StartCoroutine(ShowArtifactMessage());
    }

    IEnumerator ShowArtifactMessage()
    {
        _artifactText.enabled = true;
        yield return new WaitForSeconds(1);
        _artifactText.enabled = false;
    }

    void Start()
    {
        _gameOverText.SetActive(false);
        _artifactsCollectedText.gameObject.SetActive(false);
        _artifactsCollectedText.enabled = false;
        _restartText.SetActive(false);
        _artifactText.enabled = false;

        _broker.Receive<HealthChangeEvent>()
            .Where(e => e.Health.gameObject.GetComponent<PlayerController>() != null)
            .Subscribe(e =>
            {
                _healthFill.fillAmount = e.Health.CurrentPerc.Value;

                if (e.Health.Current.Value == 0)
                {
                    _gameOverText.SetActive(true);
                    _healthContainer.SetActive(false);
                }
            })
            .AddTo(this);

        _timer.CurrentTime
            .Subscribe(OnSecondsChange)
            .AddTo(this);

        _timer.CurrentTimeProgress
            .Subscribe(OnProgressChange)
            .AddTo(this);
    }

    void OnSecondsChange(float seconds)
    {
        _timerText.text = $"{TimeSpan.FromSeconds(seconds):ss\\.ff}";
    }

    void OnProgressChange(float progress)
    {
        // Debug.Log("Progress: " + progress);
    }
}
