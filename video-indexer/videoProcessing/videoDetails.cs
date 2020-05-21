using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace videoProcessing
{
    public class videoIndexData
    {
        public string accountId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public object partition { get; set; }
        public object externalId { get; set; }
        public object metadata { get; set; }
        public object description { get; set; }
        public string userName { get; set; }
        public DateTime created { get; set; }
        public string privacyMode { get; set; }
        public string state { get; set; }
        public bool isOwned { get; set; }
        public bool isEditable { get; set; }
        public bool isBase { get; set; }
        public int durationInSeconds { get; set; }
        public SummarizedInsights summarizedInsights { get; set; }
        public List<Video> videos { get; set; }
        public List<VideosRange> videosRanges { get; set; }
        public DateTime lastModified { get; set; }
        public DateTime lastIndexed { get; set; }
        public string processingProgress { get; set; }
        public List<object> searchMatches { get; set; }
        public string indexingPreset { get; set; }
        public string streamingPreset { get; set; }
        public string sourceLanguage { get; set; }
        public string blobPath { get; set; }
        public string thumbnailVideoId { get; set; }
        public string thumbnailId { get; set; }
    }

    public class Duration
    {
        public string time { get; set; }
        public double seconds { get; set; }
    }

    public class Appearance
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public double startSeconds { get; set; }
        public double endSeconds { get; set; }
    }

    public class Face
    {
        public int id { get; set; }
        public string videoId { get; set; }
        public object referenceId { get; set; }
        public string referenceType { get; set; }
        public string knownPersonId { get; set; }
        public double confidence { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public object title { get; set; }
        public string thumbnailId { get; set; }
        public List<Appearance> appearances { get; set; }
        public double seenDuration { get; set; }
        public double seenDurationRatio { get; set; }
        public object imageUrl { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Keyword
    {
        public int id { get; set; }
        public string text { get; set; }
        public string name { get; set; }
        public List<Appearance> appearances { get; set; }
        public bool isTranscript { get; set; }
        public double confidence { get; set; }
        public string language { get; set; }
        public List<Instance> instances { get; set; }
    }


    public class Sentiment
    {
        public int id { get; set; }
        public string sentimentKey { get; set; }
        public List<Appearance> appearances { get; set; }
        public double seenDurationRatio { get; set; }
        public double averageScore { get; set; }
        public string sentimentType { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Label
    {
        public int id { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public List<Appearance> appearances { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Statistics
    {
        public int correspondenceCount { get; set; }
        public object speakerTalkToListenRatio { get; set; }
        public object speakerLongestMonolog { get; set; }
        public object speakerNumberOfFragments { get; set; }
        public object speakerWordCount { get; set; }
    }

    public class SummarizedInsights
    {
        public string name { get; set; }
        public string id { get; set; }
        public string privacyMode { get; set; }
        public Duration duration { get; set; }
        public string thumbnailVideoId { get; set; }
        public string thumbnailId { get; set; }
        public List<Face> faces { get; set; }
        public List<Keyword> keywords { get; set; }
        public List<Sentiment> sentiments { get; set; }
        public List<object> audioEffects { get; set; }
        public List<Label> labels { get; set; }
        public List<object> brands { get; set; }
        public Statistics statistics { get; set; }
    }

    public class Instance
    {
        public double confidence { get; set; }
        public string adjustedStart { get; set; }
        public string adjustedEnd { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public List<string> thumbnailsIds { get; set; }
        public string thumbnailId { get; set; }

    }

    public class Transcript
    {
        public int id { get; set; }
        public string text { get; set; }
        public double confidence { get; set; }
        public int speakerId { get; set; }
        public string language { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Ocr
    {
        public int id { get; set; }
        public string text { get; set; }
        public double confidence { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string language { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class KeyFrame
    {
        public int id { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Shot
    {
        public int id { get; set; }
        public List<KeyFrame> keyFrames { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Block
    {
        public int id { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class FramePattern
    {
        public int id { get; set; }
        public string patternType { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class Speaker
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Instance> instances { get; set; }
    }

    public class TextualContentModeration
    {
        public int id { get; set; }
        public int bannedWordsCount { get; set; }
        public double bannedWordsRatio { get; set; }
        public List<object> instances { get; set; }
    }

    public class Insights
    {
        public string version { get; set; }
        public string duration { get; set; }
        public string sourceLanguage { get; set; }
        public string language { get; set; }
        public List<Transcript> transcript { get; set; }
        public List<Ocr> ocr { get; set; }
        public List<Keyword> keywords { get; set; }
        public List<Face> faces { get; set; }
        public List<Label> labels { get; set; }
        public List<Shot> shots { get; set; }
        public List<Sentiment> sentiments { get; set; }
        public List<Block> blocks { get; set; }
        public List<FramePattern> framePatterns { get; set; }
        public List<Speaker> speakers { get; set; }
        public TextualContentModeration textualContentModeration { get; set; }
    }

    public class Video
    {
        public string accountId { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string privacyMode { get; set; }
        public string processingProgress { get; set; }
        public string failureCode { get; set; }
        public string failureMessage { get; set; }
        public bool isAdult { get; set; }
        public object externalId { get; set; }
        public object externalUrl { get; set; }
        public object metadata { get; set; }
        public Insights insights { get; set; }
        public string thumbnailId { get; set; }
        public string publishedUrl { get; set; }
        public object publishedProxyUrl { get; set; }
        public string viewToken { get; set; }
        public string sourceLanguage { get; set; }
        public string language { get; set; }
        public string indexingPreset { get; set; }
        public string linguisticModelId { get; set; }
    }

    public class Range
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class VideosRange
    {
        public string videoId { get; set; }
        public Range range { get; set; }
    }
}
