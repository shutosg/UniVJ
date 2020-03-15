using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public static readonly Vector2Int Resolution = new Vector2Int(1280, 720);
    [SerializeField] private Shader _mainRendererShader;
    [SerializeField] private MainRendererView _rendererView;
    [SerializeField] private FootageListView _footageListView;
    [SerializeField] private ControlPanel _controlPanel;

    public override void InstallBindings()
    {
        Container.Bind<MainRenderer>().FromMethod(context => new MainRenderer(_rendererView, _mainRendererShader, Resolution)).AsSingle().NonLazy();
        Container.Bind<MainRendererView>().FromInstance(_rendererView).AsSingle().NonLazy();
        Container.Bind<FootageListView>().FromInstance(_footageListView).AsSingle().NonLazy();
        Container.Bind<LayerManager>().AsSingle().NonLazy();
        Container.Bind<ThumbnailMaker>().AsSingle().NonLazy();
    }
}