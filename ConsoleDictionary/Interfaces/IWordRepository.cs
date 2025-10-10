using ConsoleDictionary.Entities;
using ConsoleDictionary.Helpers;
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
        OperationResult Add(Word word);
        OperationResult Delete(string text);
        OperationResult Update(Word oldWord, Word newWord);
        Word? Find(string text);
        IReadOnlyList<Word> GetAll();
        OperationResult Save(string path);
        OperationResult Load(string path);
    }
}
