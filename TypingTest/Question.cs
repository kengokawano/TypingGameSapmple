using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace TypingTest
{
    public class Question
    {
        public int id { get; set; }
        public string text { get; set; }
        public string kana { get; set; }
        public List<string> tags { get; set; }
        public int era { get; set; }
    }

    public static class QuestionLoader
    {
        /// <summary>
        //* 指定されたパスのJSONファイルを読み込み、Questionオブジェクトのリストを返す
        //* </summary>
        //* <param name = "filePath" > 読み込むJSONファイルのパス </ param >
        //* < returns > Questionオブジェクトのリスト </ returns >
        public static List<Question> LoadQuestionsFromFile(string filePath)
        {
            // ファイルからJSON文字列を読み込む
            string jsonText = File.ReadAllText(filePath);

            // JSONをデシリアライズ（オブジェクトに変換）する
            var serializer = new JavaScriptSerializer();
            List<Question> questions = serializer.Deserialize<List<Question>>(jsonText);

            return questions;
        }
    }
}
