# TypingTest

これは、C# (Windows Forms) で開発された日本語ローマ字タイピングゲームのプロトタイプです。

## 主な機能

- ローマ字での日本語タイピング
- ランダムな問題文の出題機能
- プレイ時間、コンボ数のリアルタイム表示
- ゲームクリア時のスコア表示 (タイム, 最大コンボ, 総タイプ数, KPM, WPM)
- クリアタイムのランキング機能（上位10件）

## 実行方法

1.  `bin/Debug/TypingTest.exe` を実行します。
2.  表示されるウィンドウのスタートボタンを押して、ゲームを開始します。

## ライセンス (License)

このプロジェクトは [MIT License](LICENSE.txt) のもとで公開されています。

## 謝辞 (Acknowledgements)

このプロジェクトで利用しているローマ字入力パーサー (`RomanTypingParser.cs`) およびローマ字定義辞書 (`romanTypingParseDictionary.json`) は、第三者によって作成された成果物であり、MITライセンスに基づき使用しています。

- **Original Work:** [RomanTypeParser by WhiteFox-Lugh](https://github.com/WhiteFox-Lugh/RomanTypeParser)
- **Copyright:** (c) WhiteFox-Lugh
- **License:** MIT License
