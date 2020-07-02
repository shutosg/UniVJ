using UnityEngine;
using Zenject;
using TMPro;

namespace KineticTypo
{
    public enum InstallerId
    {
        MainTextId,
    }

    public class KineticTypoInstaller : MonoInstaller
    {
        [SerializeField] TextMeshPro _text;

        public override void InstallBindings()
        {
            Container.Bind<TextMeshPro>().WithId(InstallerId.MainTextId).FromInstance(_text).AsTransient().NonLazy();
        }
    }
}