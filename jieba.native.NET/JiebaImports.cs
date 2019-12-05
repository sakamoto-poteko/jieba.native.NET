using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace jieba.native.NET
{
    public static class JiebaImports
    {
        #region Context
        [DllImport("jieba-c")]
        public static extern IntPtr jieba_create_context(string dict_path, string hmm_path, string user_dict, string idf_path, string stop_words_path);

        [DllImport("jieba-c")]
        public static extern void jieba_destroy_context(IntPtr ctx);

        [DllImport("jieba-c")]
        public static extern IntPtr jieba_extractor_create_context(string dict_path, string hmm_path, string user_dict, string idf_path, string stop_words_path);

        [DllImport("jieba-c")]
        public static extern IntPtr jieba_extractor_create_context_from_jieba_context(IntPtr ctx);

        [DllImport("jieba-c")]
        public static extern void jieba_extractor_destroy_context(IntPtr ctx);
        #endregion


        #region Free
        [DllImport("jieba-c")]
        public static extern void jieba_words_free(ref JiebaWords w);

        [DllImport("jieba-c")]
        public static extern void jieba_tags_free(ref JiebaTags t);

        [DllImport("jieba-c")]
        public static extern void jieba_weights_free(ref JiebaWeights t);
        #endregion


        #region Functionality
        [DllImport("jieba-c")]
        public static extern bool jieba_insert_user_word(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string word);

        [DllImport("jieba-c")]
        public static extern bool jieba_insert_user_word_with_tag(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string word, [MarshalAs(UnmanagedType.LPUTF8Str)] string tag);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, ref JiebaWords words, bool hmm = true);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut_full(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, uint len, ref JiebaWords words, bool hmm = true);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut_all(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, ref JiebaWords words);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut_all_full(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, uint len, ref JiebaWords words);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut_for_search(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, ref JiebaWords words, bool hmm = true);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut_for_search_full(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, uint len, ref JiebaWords words, bool hmm = true);

        [DllImport("jieba-c")]
        public static extern uint jieba_cut_with_tag(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, ref JiebaTags result);

        [DllImport("jieba-c")]
        public static extern uint jieba_extract(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, uint top_n, ref JiebaWeights weights);

        [DllImport("jieba-c")]
        public static extern uint jieba_extract_full(IntPtr ctx, [MarshalAs(UnmanagedType.LPUTF8Str)]string sentence, int len, uint top_n, ref JiebaWeights weights);
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct JiebaWords
        {
            public IntPtr Words;    // char **
            public uint Count;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct JiebaTags
        {
            public IntPtr Tags;    // char **
            public IntPtr Words;   // char **
            public uint Count;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct JiebaWeights
        {
            public IntPtr Words;    // char **
            public IntPtr Weights;  // double *
            public uint Count;
        };

        public static List<string> JiebaWords2StringList(ref JiebaWords words)
        {
            var ret = new List<string>();

            for (int i = 0; i < words.Count; ++i)
            {
                var wordPtrEntryInArray = IntPtr.Add(words.Words, IntPtr.Size * i);
                var wordPtr = Marshal.PtrToStructure<IntPtr>(wordPtrEntryInArray);
                var word = Marshal.PtrToStringUTF8(wordPtr);
                ret.Add(word);
            }

            return ret;
        }

        public static List<(string word, string tag)> JiebaTags2StringList(ref JiebaTags words)
        {
            var ret = new List<(string word, string tag)>();

            for (int i = 0; i < words.Count; ++i)
            {
                var wordPtrEntryInArray = IntPtr.Add(words.Words, IntPtr.Size * i);
                var tagPtrEntryInArray = IntPtr.Add(words.Tags, IntPtr.Size * i);
                var wordPtr = Marshal.PtrToStructure<IntPtr>(wordPtrEntryInArray);
                var tagPtr = Marshal.PtrToStructure<IntPtr>(tagPtrEntryInArray);
                var word = Marshal.PtrToStringUTF8(wordPtr);
                var tag = Marshal.PtrToStringUTF8(tagPtr);
                ret.Add((word, tag));
            }

            return ret;
        }

        public static List<(string word, double weight)> JiebaWeights2StringList(ref JiebaWeights weights)
        {
            var ret = new List<(string word, double weight)>();

            for (int i = 0; i < weights.Count; ++i)
            {
                var wordPtrEntryInArray = IntPtr.Add(weights.Words, IntPtr.Size * i);
                var weightPtr = IntPtr.Add(weights.Weights, Marshal.SizeOf<double>() * i);
                var wordPtr = Marshal.PtrToStructure<IntPtr>(wordPtrEntryInArray);
                var word = Marshal.PtrToStringUTF8(wordPtr);
                var weight = Marshal.PtrToStructure<double>(weightPtr);
                ret.Add((word, weight));
            }

            return ret;
        }
    }
}
