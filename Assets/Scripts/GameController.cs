using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : MonoBehaviour
{
    [SerializeField]
    AudioClip _roundStartSfx;

    [SerializeField]
    AudioClip _gameOverSfx;

    [SerializeField]
    AudioClip _pickUpSfx;

    [SerializeField]
    GameUI _gameUI;

    [SerializeField]
    List<Transform> _dropPoints;

    [SerializeField]
    List<ArtifactData> _artifactSources;

    Artifact.Factory _factory;

    IMessageBroker _broker;
    IAudioController _audio;
    int _stage = 0;
    List<EnemySpawner> _spawners = new List<EnemySpawner>();
    IReactiveProperty<int> _enemiesKilled = new ReactiveProperty<int>(0);
    bool _playerDead = false;
    bool _allowRestart = false;
    List<ArtifactData> _pickedUpArtifacts = new List<ArtifactData>();

    [Inject]
    void Inject(IMessageBroker broker, IAudioController audio, Artifact.Factory factory)
    {
        _broker = broker;
        _audio = audio;
        _factory = factory;
    }

    void Start()
    {
        _audio.PlayGameMusic();
        _spawners.AddRange(FindObjectsOfType<EnemySpawner>());
        _broker.Receive<TenSecondsPassedEvent>()
            .Subscribe(OnTenSeconds)
            .AddTo(this);

        _broker.Receive<PickupArtifactEvent>()
            .Subscribe(e =>
            {
                _pickedUpArtifacts.Add(e.Data);
                _audio.Play(_pickUpSfx, 0.9f, 1);
                _gameUI.ShowPickedUpItem(e.Data);

                // when you collect an artifact, drop a new one
                SpawnArtifact();
            })
            .AddTo(this);

        InputSystem.onAnyButtonPress
            .Subscribe(_ =>
            {
                if (_allowRestart)
                    SceneManager.LoadScene("SampleScene");
            })
            .AddTo(this);

        _broker.Receive<EntityDiedEvent>()
            .Subscribe(e =>
            {
                if (e.DiedObject.GetComponent<PlayerController>() != null)
                {
                    _playerDead = true;
                    _audio.Play(_gameOverSfx, 1, 1);
                    _gameUI.ShowArtifactsCollectedText(_pickedUpArtifacts);
                    StartCoroutine(WaitEnableRestart());
                }
                else
                {
                    ++_enemiesKilled.Value;
                }
            })
            .AddTo(this);

        _enemiesKilled
            .Subscribe(_gameUI.SetEnemiesKilled)
            .AddTo(this);

        // start with one
        SpawnArtifact();
    }

    void OnTenSeconds(TenSecondsPassedEvent e)
    {
        if (_playerDead)
            return;

        _stage++;

        _broker.Publish(new GameStageProgressEvent(_stage));
        _audio.Play(_roundStartSfx, 0.9f, Random.Range(0.9f, 1));

        int max = 15;
        int spawn = Mathf.Min(_stage, max);
        for (int i = 0; i < spawn; ++i)
        {
            var index = Random.Range(0, _spawners.Count - 1);
            _spawners[index].Spawn();
        }
    }

    IEnumerator WaitEnableRestart()
    {
        yield return new WaitForSeconds(1.0f);
        _allowRestart = true;
        _gameUI.ShowRestartText();
    }

    void SpawnArtifact()
    {
        // pick a point
        var pointIndex = Random.Range(0, _dropPoints.Count - 1);
        var point = _dropPoints[pointIndex];

        // pick an artifact
        var artifactIndex = Random.Range(0, _artifactSources.Count - 1);
        var artifactSource = _artifactSources[artifactIndex];

        // spawn at point
        var artifact = _factory.Create();
        artifact.SetData(artifactSource);
        artifact.transform.position = point.position;
    }
}
