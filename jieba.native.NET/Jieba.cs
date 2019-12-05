using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace jieba.native.NET
{
    public class Jieba
    {
        public Jieba(string dictPath, string hmmPath, string userDict, string idfPath, string stopWordPath)
        {
            JiebaContext = JiebaImports.jieba_create_context(dictPath, hmmPath, userDict, idfPath, stopWordPath);

            if (JiebaContext == IntPtr.Zero)
            {
                throw new ArgumentException();
            }

            _extractor = new Lazy<Extractor>(() => new Extractor(this));
        }

        ~Jieba()
        {
            JiebaImports.jieba_destroy_context(JiebaContext);
        }

        internal IntPtr JiebaContext { get; }

        private readonly Lazy<Extractor> _extractor;

        public Extractor Extractor => _extractor.Value;


        public List<string> Cut(string sentence, bool hmm = true)
        {
            var words = new JiebaImports.JiebaWords();
            JiebaImports.jieba_cut(JiebaContext, sentence, ref words, hmm);

            var ret = JiebaImports.JiebaWords2StringList(ref words);
            JiebaImports.jieba_words_free(ref words);
            return ret;
        }

        public List<string> CutAll(string sentence)
        {
            var words = new JiebaImports.JiebaWords();
            JiebaImports.jieba_cut_all(JiebaContext, sentence, ref words);
            var ret = JiebaImports.JiebaWords2StringList(ref words);
            JiebaImports.jieba_words_free(ref words);
            return ret;
        }

        public List<string> CutForSearch(string sentence, bool hmm = true)
        {
            var words = new JiebaImports.JiebaWords();
            JiebaImports.jieba_cut_for_search(JiebaContext, sentence, ref words, hmm);
            var ret = JiebaImports.JiebaWords2StringList(ref words);
            JiebaImports.jieba_words_free(ref words);
            return ret;
        }

        public List<(string word, string tag)> Tag(string sentence)
        {
            var tags = new JiebaImports.JiebaTags();
            JiebaImports.jieba_cut_with_tag(JiebaContext, sentence, ref tags);
            var ret = JiebaImports.JiebaTags2StringList(ref tags);
            JiebaImports.jieba_tags_free(ref tags);
            return ret;
        }

    }
}
