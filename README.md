# スライドリ

## ゲーム概要

* ゲーム上に配置されたとりのブロックを左右にスライドさせて、横１列を揃えるパズルゲームです
* 自身のブロックの下が空きマスの場合は落下します
* プレイヤーの操作後ランダムな1~4マスの空白を含む1列のブロックが一番下の列に生成され、すでにボード上にあるブロックは1列分上昇します。
* また、上昇後にブロックが削除された場合は再度１列分上昇します。
* 盤面上のブロックが一番上の列まで達したらゲームオーバーです。

## 操作方法

ブロックを長押しした状態で左右に移動させるだけ！

## 主な使用ライブラリ

* UniRx: GUI周りのアーキテクチャとしてMV(R)Pアーキテクチャを実現するために使用しました。  
参考: [【Unity】Model-View-(Reactive)Presenterパターンとは何なのか](https://qiita.com/toRisouP/items/5365936fc14c7e7eabf9)

* UniTask: DOTweenのアニメーション終了を待機する非同期処理の実装に使用しました。

* DOTween: アニメーションを作るために使用しました。

## デモ版

以下のリンクからUnityRoomでプレイすることができます！
https://unityroom.com/games/slide-bird

## 参考にしたゲーム

Slidey®：ブロックパズル  
https://play.google.com/store/apps/details?id=com.mavis.slidey&hl=ja
