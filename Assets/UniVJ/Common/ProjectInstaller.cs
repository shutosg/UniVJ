using UnityEngine;
using UnityEngine.Video;
using Zenject;
using UniVJ;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<FootageManager>().AsSingle().NonLazy();
    }
}