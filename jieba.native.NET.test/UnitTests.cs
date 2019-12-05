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

        private const string TestSentence1 = "他来到了网易杭研大厦";
        private const string TestSentence2 = "我来到北京清华大学";
        private const string TestSentence3 = "小明硕士毕业于中国科学院计算所，后在日本京都大学深造";
        private const string TestSentence4 = "我是拖拉机学院手扶拖拉机专业的。不用多久，我就会升职加薪，当上CEO，走上人生巅峰。";
        public UnitTests(JiebaFixture jieba)
        {
            _jieba = jieba;
        }

        [Fact]
        public void TestCutWithHmm()
        {
            var result = _jieba.Jieba.Cut(TestSentence1);
            Assert.Equal(6, result.Count);
            Assert.Equal("他/来到/了/网易/杭研/大厦", string.Join('/', result));
        }

        [Fact]
        public void TestCutWithoutHmm()
        {
            var result = _jieba.Jieba.Cut(TestSentence1, false);
            Assert.Equal(7, result.Count);
            Assert.Equal("他/来到/了/网易/杭/研/大厦", string.Join('/', result));
        }

        [Fact]
        public void TestCutAll()
        {
            var result = _jieba.Jieba.CutAll(TestSentence2);
            Assert.Equal(7, result.Count);
            Assert.Equal("我/来到/北京/清华/清华大学/华大/大学", string.Join('/', result));
        }

        [Fact]
        public void TestCutForSearch()
        {
            var result = _jieba.Jieba.CutForSearch(TestSentence3);
            Assert.Equal(19, result.Count);
            Assert.Equal("小明/硕士/毕业/于/中国/科学/学院/科学院/中国科学院/计算/计算所/，/后/在/日本/京都/大学/日本京都大学/深造", string.Join('/', result));
        }

        [Fact]
        public void TestTag()
        {
            var result = _jieba.Jieba.Tag(TestSentence4);
            var l = result.Select(((string, string) wordTag) => $"{wordTag.Item1}:{wordTag.Item2}").ToList();
            var s = string.Join(", ", l);
            Assert.Equal(24, result.Count);
            Assert.Equal("我:r, 是:v, 拖拉机:n, 学院:n, 手扶拖拉机:n, 专业:n, 的:uj, 。:x, 不用:v, 多久:m, ，:x, 我:r, 就:d, 会:v, 升职:v, 加薪:nr, ，:x, 当上:t, CEO:eng, ，:x, 走上:v, 人生:n, 巅峰:n, 。:x", s);
        }

        [Fact]
        public void TestExtract()
        {
            var result = _jieba.Jieba.Extractor.Extract(TestSentence4, 5);
            var s = string.Join('/', result.Select((t) => t.Item1));
            Assert.Equal(5, result.Count);
            Assert.Equal("CEO/升职/加薪/手扶拖拉机/巅峰", s);
            Assert.True(result[0].Item2 > result[1].Item2);
            Assert.True(result[1].Item2 > result[2].Item2);
            Assert.True(result[2].Item2 > result[3].Item2);
            Assert.True(result[3].Item2 > result[4].Item2);
        }
    }
}
