using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using notebook.Models;

namespace notebook.DataLayer.Abstracts
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotes(string userId);
        Task<Note> GetNote(string id, string userId);
        Task<bool> AddNote(Note model);
        Task<bool> UpdateNote(Note model, string userId, string id);
        Task<DeleteResult> RemoveNote(string id, string userId);
        Task<DeleteResult> RemoveAllNotes(string userId);
    }
}