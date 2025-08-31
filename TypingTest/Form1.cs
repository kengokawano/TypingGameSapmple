using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TypingTest
{
    public partial class Form1 : Form
    {
        //================================================================
        // ゲーム全体の状態管理
        //================================================================

        /// <summary>
        /// ゲームが開始されているかどうか
        /// </summary>
        private bool isGameStarted = false;

        /// <summary>
        /// ゲームプレイ時間を計測するタイマー
        /// </summary>
        private Timer gameTimer;

        /// <summary>
        /// 経過時間（秒）
        /// </summary>
        private int elapsedTimeInSeconds = 0;


        //================================================================
        // スコア・ランキング関連
        //================================================================

        /// <summary>
        /// 現在のコンボ数
        /// </summary>
        private int comboCount = 0;

        /// <summary>
        /// ゲーム中の総キー入力数
        /// </summary>
        private int totalKeyPresses = 0;

        /// <summary>
        /// クリアタイムのランキング（昇順）
        /// </summary>
        private List<int> ranking;


        //================================================================
        // 問題文関連
        //================================================================

        /// <summary>
        /// 問題文のリスト
        /// </summary>
        private List<string> questions;

        /// <summary>
        /// 現在の問題文（ひらがな）
        /// </summary>
        private string currentQuestionHiragana;

        /// <summary>
        /// 問題文をランダムに選択するためのジェネレーター
        /// </summary>
        private Random random = new Random();


        //================================================================
        // 現在のタイピング処理の状態
        //================================================================

        /// <summary>
        /// 現在の問題文を「かな」単位に分解したリスト
        /// </summary>
        private List<string> _currentKana;

        /// <summary>
        /// 現在の問題文の各かなに対応するローマ字リスト
        /// </summary>
        private List<List<string>> _currentRoman;

        /// <summary>
        /// 現在入力対象の「かな」のインデックス
        /// </summary>
        private int _currentKanaIndex = 0;

        /// <summary>
        /// 現在入力中のローマ字候補（キー入力で絞り込まれる）
        /// </summary>
        private List<string> _candidateRomans;

        /// <summary>
        /// 現在のローマ字の何文字目を入力しているか
        /// </summary>
        private int _inputRomanIndex = 0;


        //================================================================
        // 初期化処理
        //================================================================

        public Form1()
        {
            InitializeComponent();
            RomanTypingParserJp.ReadJsonFile();
            InitializeGame();
        }

        private void InitializeGame()
        {
            gameTimer = new Timer();
            gameTimer.Interval = 1000; // 1秒ごとにTickイベントを発生
            gameTimer.Tick += GameTimer_Tick;

            ranking = new List<int>();
            LoadQuestions();
            UpdateDisplay(); // 初期表示
        }

        private void LoadQuestions()
        {
            // ここに問題文を追加します
            questions = new List<string>
            {
                "こんにちはせかい",
                "これはたいぴんぐげーむです",
                "ぷろぐらみんぐはたのしい",
                "やまとなでしこしちへんげ",
                "すもももももももものうち"
            };
        }

        //================================================================
        // ゲームフロー（開始・終了・更新）
        //================================================================

        private void StartGame()
        {
            // クリアメッセージが残っている場合があるので初期化
            laKana.Text = "";
            isGameStarted = true;
            elapsedTimeInSeconds = 0;
            comboCount = 0;
            totalKeyPresses = 0;

            // ランダムに問題を選択
            currentQuestionHiragana = questions[random.Next(questions.Count)];

            // 問題文を解析
            (var kana, var roman) = RomanTypingParserJp.ConstructTypeSentence(currentQuestionHiragana);
            _currentKana = kana;
            _currentRoman = roman;

            // タイピング状態をリセット
            _currentKanaIndex = 0;
            _inputRomanIndex = 0;
            _candidateRomans = null;

            // タイマースタート
            gameTimer.Start();

            UpdateDisplay();
            this.ActiveControl = null;
        }

        private void FinishGame()
        {
            isGameStarted = false;
            gameTimer.Stop();

            // スコア計算
            double kpm = 0;
            double wpm = 0;
            if (elapsedTimeInSeconds > 0)
            {
                kpm = (double)totalKeyPresses / elapsedTimeInSeconds * 60;
                wpm = (double)totalKeyPresses / 5.0 / (elapsedTimeInSeconds / 60.0);
            }

            var sb = new StringBuilder();
            sb.AppendLine("🎉🎉🎉 ゲームクリア！ 🎉🎉🎉");
            sb.AppendLine();
            sb.AppendLine($"タイム: {elapsedTimeInSeconds} 秒");
            sb.AppendLine($"最大コンボ: {comboCount}");
            sb.AppendLine($"総タイプ数: {totalKeyPresses}");
            sb.AppendLine($"KPM (打/分): {kpm:F2}");
            sb.AppendLine($"WPM (語/分): {wpm:F2}");
            laKana.Text = sb.ToString();

            // ランキング処理
            UpdateRanking(elapsedTimeInSeconds);
            ShowRanking();
        }

        private void UpdateRanking(int score)
        {
            ranking.Add(score);
            ranking.Sort(); // 昇順（タイムが短いほど上位）

            // 上位10件までを保持
            if (ranking.Count > 10)
            {
                ranking = ranking.Take(10).ToList();
            }
        }

        private void ShowRanking()
        {
            var sb = new StringBuilder();
            sb.AppendLine("🏆 ランキング 🏆");
            sb.AppendLine("---------------------");

            if (ranking.Count == 0)
            {
                sb.AppendLine("まだ記録がありません。");
            }
            else
            {
                for (int i = 0; i < ranking.Count; i++)
                {
                    sb.AppendLine($"{(i + 1)}位: {ranking[i]} 秒");
                }
            }

            MessageBox.Show(sb.ToString(), "ランキング");
        }

        private void UpdateDisplay()
        {
            if (!isGameStarted)
            {
                // ゲーム開始前か、ゲーム終了後
                if (laKana.Text.StartsWith("🎉")) // クリアメッセージが表示されている場合
                {
                    // そのままにする
                    return;
                }
                laKana.Text = "スタートボタンを押してゲームを開始してください";
                textBoxInput.Text = ""; // 前回の入力をクリア
                labelOutput.Text = ""; // 前回の出力をクリア
                return;
            }

            // --- ゲーム中の表示 ---
            var sb = new StringBuilder();

            // 1. 問題文全体の表示 (入力済み/未入力)
            var completedKana = string.Join("", _currentKana.Take(_currentKanaIndex));
            var remainingKana = string.Join("", _currentKana.Skip(_currentKanaIndex));

            // 2. 現在のターゲット（かな＋ローマ字）
            var currentKana = this.CurrentKana();
            var currentRomans = this.CurrentRoman();
            string romanText;

            if (_candidateRomans != null && _candidateRomans.Any() && _inputRomanIndex > 0)
            {
                // 入力中の場合、入力済み部分と未入力部分を分けて表示
                var formattedCandidates = _candidateRomans.Select(r =>
                {
                    var completed = r.Substring(0, _inputRomanIndex);
                    var remaining = r.Substring(_inputRomanIndex);
                    return $"[{completed}]{remaining}"; // 例: [k]a
                });
                romanText = string.Join(", ", formattedCandidates);
            }
            else
            {
                // 初期状態の場合、すべての候補を表示
                romanText = string.Join(", ", currentRomans);
            }

            // 3. 統計情報
            var statsText = $"Time: {elapsedTimeInSeconds}s | Combo: {comboCount}";

            // 全てを結合して表示
            labelOutput.Text = $"問題: {completedKana}【{remainingKana}】";
            laKana.Text = $"{currentKana} : {romanText}";
            textBoxInput.Text = statsText; // 便宜的にtextBoxInputに統計情報を表示
        }

        //================================================================
        // イベントハンドラ
        //================================================================

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            elapsedTimeInSeconds++;
            UpdateDisplay();
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            // ボタンの役割をゲームスタートに変更
            StartGame();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ゲームが始まっていなければ何もしない
            if (!isGameStarted)
            {
                return;
            }

            // バックスペースやEnterなどの制御文字は無視する
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            totalKeyPresses++; // 有効なキー入力をカウント

            // KeyPressEventArgsから入力された文字を直接取得 ✨
            var inputChar = e.KeyChar.ToString();

            // 1. 最初の文字の入力処理 (_inputRomanIndexが0のとき)
            if (_inputRomanIndex == 0)
            {
                var allRomans = this.CurrentRoman();

                // 入力された文字で始まる候補をすべて検索する
                _candidateRomans = allRomans
                    .Where(r => r.StartsWith(inputChar))
                    .ToList();

                // 候補が見つかった場合
                if (_candidateRomans.Any())
                {
                    _inputRomanIndex = 1; // 次の文字へ
                }
                else
                {
                    comboCount = 0; // タイプミス
                }
            }
            // 2. 二文字目以降の入力処理
            else
            {
                // 現在の候補の中から、さらに次の文字が一致するものを探す
                var nextCandidates = _candidateRomans
                    .Where(r => r.Length > _inputRomanIndex && r[_inputRomanIndex].ToString() == inputChar)
                    .ToList();

                // 候補が見つかった場合
                if (nextCandidates.Any())
                {
                    _candidateRomans = nextCandidates; // 候補を更新
                    _inputRomanIndex++;               // 次の文字へ
                }
                else
                {
                    // タイプミス：状態をリセット
                    _inputRomanIndex = 0;
                    _candidateRomans = null;
                    comboCount = 0; // タイプミス
                }
            }

            // 3. かな入力完了の判定
            if (_candidateRomans != null && _candidateRomans.Any(r => r.Length == _inputRomanIndex))
            {
                comboCount++; // コンボ加算
                // 問題のインデックスを次に進める
                _currentKanaIndex++;

                // 次のかなのために状態をリセット
                _inputRomanIndex = 0;
                _candidateRomans = null;

                // もし全ての問題が終わったら...
                if (_currentKanaIndex >= _currentKana.Count)
                {
                    FinishGame();
                    return;
                }
            }

            // 4. 表示の更新
            UpdateDisplay();
        }

        //================================================================
        // ヘルパーメソッド
        //================================================================

        private string CurrentKana()
        {
            if (!isGameStarted || _currentKana == null || _currentKanaIndex >= _currentKana.Count)
            {
                return "";
            }
            return this._currentKana[this._currentKanaIndex];
        }

        private List<string> CurrentRoman()
        {
            if (!isGameStarted || _currentRoman == null || _currentKanaIndex >= _currentRoman.Count)
            {
                return new List<string>();
            }
            return this._currentRoman[this._currentKanaIndex];
        }
    }
}
