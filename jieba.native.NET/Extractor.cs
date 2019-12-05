using System;
using System.Collections.Generic;
using System.Text;

namespace jieba.native.NET
{
    public class Extractor
    {
        public Extractor(string dictPath, string hmmPath, string userDict, string idfPath, string stopWordPath)
        {
            _extractor = JiebaImports.jieba_extractor_create_context(dictPath, hmmPath, userDict, idfPath, stopWordPath);
            if (_extractor == IntPtr.Zero)
            {
                throw new ArgumentException();
            }
        }

        internal Extractor(Jieba jieba)
        {
            _jieba = jieba;
            _extractor = JiebaImports.jieba_extractor_create_context_from_jieba_context(jieba.JiebaContext);
            if (_extractor == IntPtr.Zero)
            {
                throw new ArgumentException();
            }
        }

        ~Extractor()
        {
            JiebaImports.jieba_extractor_destroy_context(_extractor);
        }

        private readonly Jieba _jieba;
        private readonly IntPtr _extractor;

        public List<(string, double)> Extract(string sentence, uint topN)
        {
            var weights = new JiebaImports.JiebaWeights();
            JiebaImports.jieba_extract(_extractor, sentence, topN, ref weights);
            var result = JiebaImports.JiebaWeights2StringList(ref weights);
            JiebaImports.jieba_weights_free(ref weights);
            return result;
        }
    }
}
