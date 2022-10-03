using UniRx;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField]
    Projectile _projectileSource;

    [SerializeField]
    Enemy _enemySource;

    [SerializeField]
    Artifact _artifactSource;

    public override void InstallBindings()
    {
        // General
        Container.Bind<IMessageBroker>()
            .FromInstance(MessageBroker.Default)
            .AsSingle();

        // Audio
        Container.Bind<IAudioController>()
            .FromMethod(() => FindObjectOfType<AudioController>())
            .AsSingle();

        // Time
        Container.Bind<IGameTimer>().FromComponentInHierarchy().AsSingle();

        // Factories
        Container.BindFactory<Projectile, Projectile.Factory>()
            .FromComponentInNewPrefab(_projectileSource)
            .AsSingle();

        Container.BindFactory<Enemy, Enemy.Factory>()
            .FromComponentInNewPrefab(_enemySource)
            .AsSingle();

        Container.BindFactory<Artifact, Artifact.Factory>()
            .FromComponentInNewPrefab(_artifactSource)
            .AsSingle();
    }
}