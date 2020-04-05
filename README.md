# UniVJ
標準的な機能と柔軟な拡張性を備えたUnity製のVJソフトです

VJ Software with standard features and flexible extensibility which is made with Unity

## 機能 / Functions
* 複数ビデオファイルのミキシング / multiple video mixing
* 外部のフッテージファイルの読み込み / external footage importing
  * `/Users/<username>/UniVJ/Footages/` 配下のファイルを自動的に読み込みます
  * automatically load the media at `/Users/<username>/UniVJ/Footages/`
* Unityのシーンファイルをビデオフッテージとして読み込み / import .unity as a video footage
  * HDRPやVFXGraphを用いた高品質なリアルタイムエフェクトシーンもミキシング可能
  * can also mix high quality realtime effect scene using Unity HDRP, VFXGraph, ShaderGraph and so on
* MIDIコントローラ、マイクの入力による操作 / UI Operation by MIDI controller or Microphone inputs
  * ビデオフェード、色調補正のパラメータ調整など / Video mixing fade or color adjustment parameter

## システム要件 / System requirements
* 現状Macのみ対応 / currently support only Mac
  * いずれWindows対応予定 / someday will support Win
* Unity 2019.3 or later

## 外部依存 / External dependencies
* [DOTween](http://dotween.demigiant.com/)
* [FancyScrollView](https://github.com/setchi/FancyScrollView)
* [HSVPicker](https://github.com/judah4/HSV-Color-Picker-Unity)
* [Lasp](https://github.com/keijiro/Lasp)
* [LaspVFX](https://github.com/keijiro/LaspVfx)
* [MidiJack](https://github.com/keijiro/MidiJack)
* [SerializableDictionary](https://github.com/azixMcAze/Unity-SerializableDictionary)
* [UniRx](https://github.com/neuecc/UniRx)
* [UniTask](https://github.com/Cysharp/UniTask)
* [Extenject](https://github.com/svermeulen/Extenject)
