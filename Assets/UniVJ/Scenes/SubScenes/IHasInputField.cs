using System;
using UniRx;

public interface IHasInputField
{
    IObservable<Unit> OnStartInput { get; }
    IObservable<Unit> OnFinishInput { get; }
    bool Inputting { get; }
}