// 参照に System.Web.Extensions.dll を追加してください
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

public class Question
{
    public int id { get; set; }
    public string text { get; set; }
    public string kana { get; set; }
    public List<string> tags { get; set; }
    public int era { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string csvPath = "questions.csv";
        string jsonPath = "questions.json";

        Console.WriteLine("CSVからJSONへの変換を開始します...");

        try
        {
            var questions = new List<Question>();
            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            foreach (var line in lines.Skip(1)) // 1行目（ヘッダー）をスキップ
            {
                // ★★★ ここから修正箇所 ★★★
                var values = ParseCsvLine(line); // 新しいCSV解析メソッドを使用

                if (values.Count < 5) continue; // データが不十分な行はスキップ

                var question = new Question
                {
                    id = int.Parse(values[0]),
                    text = values[1],
                    kana = values[2],
                    // ★★★ タグの分割処理を修正（カンマで分割） ★★★
                    tags = values[3].Split(',').Select(tag => tag.Trim()).ToList(),
                    era = int.Parse(values[4])
                };
                questions.Add(question);
            }

            var serializer = new JavaScriptSerializer();
            // 見やすいようにインデントされたJSONを出力
            var jsonString = serializer.Serialize(questions);
            // 簡単な整形処理
            jsonString = jsonString.Replace("},{", "},\r\n  {").Replace("[{", "[\r\n  {").Replace("}]", "}\r\n]");


            File.WriteAllText(jsonPath, jsonString, Encoding.UTF8);

            Console.WriteLine($"変換が完了しました！ {questions.Count} 件のデータを '{jsonPath}' に保存しました。");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("エラーが発生しました。");
            Console.WriteLine(ex.Message);
            Console.ResetColor();
        }

        Console.WriteLine("何かキーを押すと終了します。");
        Console.ReadKey();
    }

    /// <summary>
    /// ダブルクォーテーションで囲まれたカンマを無視してCSV行を解析するメソッド
    /// </summary>
    public static List<string> ParseCsvLine(string line)
    {
        var result = new List<string>();
        var currentVal = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentVal.ToString());
                currentVal.Clear();
            }
            else
            {
                currentVal.Append(c);
            }
        }
        result.Add(currentVal.ToString());
        return result;
    }
}