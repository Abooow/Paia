using System;
using System.Text;

namespace TutorialSample.Services
{
    internal class RandomSentenceGenerator : IRandomSentenceGenerator
    {
        private static readonly Random random = new Random();

        private static readonly string[] words = new string[]
        {
            "umbrella",
            "result",
            "relative",
            "tone",
            "he",
            "image",
            "load",
            "thoughtful",
            "smell",
            "leaf",
            "linear",
            "beautiful",
            "occupy",
            "length",
            "slime",
            "favour",
            "bounce",
            "blow",
            "migration",
            "liberal"
        };

        public string GetRandomSentence()
        {
            return GetRandomSentence(random.Next(3, 10));
        }

        public string GetRandomSentence(int wordCount)
        {
            // Indeed a random sentence.
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < wordCount; i++)
            {
                builder.Append(words[random.Next(words.Length)] + ' ');
            }

            return builder.ToString();
        }
    }
}
