using UnityEngine;
using Zenject;

public class SubSceneInstaller : MonoInstaller
{
    [SerializeField] private SubSceneManager _subSceneManager;
    public override void InstallBindings()
    {
        Container.Bind<SubSceneManager>().FromInstance(_subSceneManager).AsTransient().NonLazy();
    }
}