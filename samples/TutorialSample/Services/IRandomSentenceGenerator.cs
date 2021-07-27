namespace TutorialSample.Services
{
    interface IRandomSentenceGenerator
    {
        string GetRandomSentence();
        string GetRandomSentence(int wordCount);
    }
}
