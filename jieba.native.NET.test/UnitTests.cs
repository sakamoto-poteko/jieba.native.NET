using System;
using System.Linq;
using Xunit;

namespace jieba.native.NET.test
{
    public class JiebaFixture : IDisposable
    {
        const string DICT_PATH = @"E:\jiebadict\dict\jieba.dict.utf8";
        const string HMM_PATH = @"E:\jiebadict\dict\hmm_model.utf8";
        const string USER_DICT = @"E:\jiebadict\dict\user.dict.utf8";
        const string IDF_PATH = @"E:\jiebadict\dict\idf.utf8";
        const string STOP_WORDS_PATH = @"E:\jiebadict\dict\stop_words.utf8";

        public Jieba Jieba { get; private set; }

        public JiebaFixture()
        {
            Jieba = new Jieba(DICT_PATH, HMM_PATH, USER_DICT, IDF_PATH, STOP_WORDS_PATH);
        }

        public void Dispose()
        {
        }
    }

    public class UnitTests : IClassFixture<JiebaFixture>
    {
        private readonly JiebaFixture _jieba;

        private const string TestSentence1 = "�����������׺��д���";
        private const string TestSentence2 = "�����������廪��ѧ";
        private const string TestSentence3 = "С��˶ʿ��ҵ���й���ѧԺ�������������ձ�������ѧ����";
        private const string TestSentence4 = "����������ѧԺ�ַ�������רҵ�ġ����ö�ã��Ҿͻ���ְ��н������CEO�����������۷塣";
        public UnitTests(JiebaFixture jieba)
        {
            _jieba = jieba;
        }

        [Fact]
        public void TestCutWithHmm()
        {
            var result = _jieba.Jieba.Cut(TestSentence1);
            Assert.Equal(6, result.Count);
            Assert.Equal("��/����/��/����/����/����", string.Join('/', result));
        }

        [Fact]
        public void TestCutWithoutHmm()
        {
            var result = _jieba.Jieba.Cut(TestSentence1, false);
            Assert.Equal(7, result.Count);
            Assert.Equal("��/����/��/����/��/��/����", string.Join('/', result));
        }

        [Fact]
        public void TestCutAll()
        {
            var result = _jieba.Jieba.CutAll(TestSentence2);
            Assert.Equal(7, result.Count);
            Assert.Equal("��/����/����/�廪/�廪��ѧ/����/��ѧ", string.Join('/', result));
        }

        [Fact]
        public void TestCutForSearch()
        {
            var result = _jieba.Jieba.CutForSearch(TestSentence3);
            Assert.Equal(19, result.Count);
            Assert.Equal("С��/˶ʿ/��ҵ/��/�й�/��ѧ/ѧԺ/��ѧԺ/�й���ѧԺ/����/������/��/��/��/�ձ�/����/��ѧ/�ձ�������ѧ/����", string.Join('/', result));
        }

        [Fact]
        public void TestTag()
        {
            var result = _jieba.Jieba.Tag(TestSentence4);
            var l = result.Select(((string, string) wordTag) => $"{wordTag.Item1}:{wordTag.Item2}").ToList();
            var s = string.Join(", ", l);
            Assert.Equal(24, result.Count);
            Assert.Equal("��:r, ��:v, ������:n, ѧԺ:n, �ַ�������:n, רҵ:n, ��:uj, ��:x, ����:v, ���:m, ��:x, ��:r, ��:d, ��:v, ��ְ:v, ��н:nr, ��:x, ����:t, CEO:eng, ��:x, ����:v, ����:n, �۷�:n, ��:x", s);
        }

        [Fact]
        public void TestExtract()
        {
            var result = _jieba.Jieba.Extractor.Extract(TestSentence4, 5);
            var s = string.Join('/', result.Select((t) => t.Item1));
            Assert.Equal(5, result.Count);
            Assert.Equal("CEO/��ְ/��н/�ַ�������/�۷�", s);
            Assert.True(result[0].Item2 > result[1].Item2);
            Assert.True(result[1].Item2 > result[2].Item2);
            Assert.True(result[2].Item2 > result[3].Item2);
            Assert.True(result[3].Item2 > result[4].Item2);
        }
    }
}
