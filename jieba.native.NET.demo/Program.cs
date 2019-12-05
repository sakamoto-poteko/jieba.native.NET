using System;

namespace jieba.native.NET.demo
{
    class Program
    {
        const string DictPath = @"E:\jiebadict\dict\jieba.dict.utf8";
        const string HmmPath = @"E:\jiebadict\dict\hmm_model.utf8";
        const string UserDict = @"E:\jiebadict\dict\user.dict.utf8";
        const string IdfPath = @"E:\jiebadict\dict\idf.utf8";
        const string StopWordsPath = @"E:\jiebadict\dict\stop_words.utf8";

        static void Main(string[] args)
        {
            var jieba = new Jieba(DictPath, HmmPath, UserDict, IdfPath, StopWordsPath);
            var result = jieba.Cut("我可以吞下玻璃而不伤身体");
            foreach (var seg in result)
            {
                Console.Write(seg);
                Console.Write('/');
            }
            Console.WriteLine();

            var tags = jieba.Tag("我是拖拉机学院手扶拖拉机专业的。不用多久，我就会升职加薪，当上CEO，走上人生巅峰。");
            foreach (var (word, tag) in tags)
            {
                Console.Write($"{word}:{tag} ");
            }
            Console.WriteLine();

            var weights = jieba.Extractor.Extract("我是拖拉机学院手扶拖拉机专业的。不用多久，我就会升职加薪，当上CEO，走上人生巅峰。", 5);
            foreach (var (word, weight) in weights)
            {
                Console.Write($"{word}:{weight} ");
            }
            Console.WriteLine();
        }
    }
}
