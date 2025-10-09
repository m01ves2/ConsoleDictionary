using ConsoleDictionary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDictionary.Interfaces
{
    public interface IWordRepository
    {
        bool IsModified { get; }
        void Add(Word word);
        void Delete(string text);
        Word? Find(string text);

        //void Update(Word oldWord, Word newWord);
        
        IReadOnlyList<Word> GetAll();
        void Save(string path);
        void Load(string path);
    }
}
