namespace GameData.Words
{
    public abstract class Verb
    {
        public abstract void Form(Word word);
        public abstract void Break(Word word);
    }
}