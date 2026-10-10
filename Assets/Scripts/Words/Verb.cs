using System.Collections.Generic;

namespace GameData.Words
{
    public abstract class Verb
    {
        protected List<Word> words = new();
        public bool Contains(Word word) => words.Contains(word);
        public abstract void Form(Word word);
        public abstract void Break(Word word);
    }
}