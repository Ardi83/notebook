using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using notebook.DataLayer.Abstracts;
using notebook.Models;

namespace notebook.DataLayer
{
    public class NoteService : INoteService
    {
        private readonly MongoRepository _repository = null;

        public NoteService(IOptions<Settings> settings)
        {
            _repository = new MongoRepository(settings);
        }

        public async Task<IEnumerable<Note>> GetAllNotes(string userId)
        {
            var result = await _repository.Notes.Find(x => x.UserId == ObjectId.Parse(userId)).ToListAsync();
            return (result);
        }

        public async Task<Note> GetNote(string id, string userId)
        {
            var filter = Builders<Note>.Filter.And(
                Builders<Note>.Filter.Eq(n => n.Id, ObjectId.Parse(id)),
                Builders<Note>.Filter.Eq(n => n.UserId, ObjectId.Parse(userId))
            );
            return await _repository.Notes.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> AddNote(Note model)
        {
            var userFilter = Builders<ApplicationUser>.Filter.Eq(u => u.Id, model.UserId);
            var user = await _repository.Users.Find(userFilter).FirstOrDefaultAsync();
            var categoryFilter = Builders<Category>.Filter.Eq(c => c.Id, model.Category.Id);
            var category = await _repository.Categories.Find(categoryFilter).FirstOrDefaultAsync();
            if (user == null || category == null)
                return false;
            model.Category = category;
            await _repository.Notes.InsertOneAsync(model);
            return true;
        }

        public async Task<bool> UpdateNote(Note model, string userId, string id)
        {
            var categoryFilter = Builders<Category>.Filter.Eq(c => c.Id, model.Category.Id);
            var category = await _repository.Categories.Find(categoryFilter).FirstOrDefaultAsync();
            var filter = Builders<Note>.Filter.And(
                Builders<Note>.Filter.Eq(n => n.Id, ObjectId.Parse(id)),
                Builders<Note>.Filter.Eq(n => n.UserId, ObjectId.Parse(userId))
            );
            var note = _repository.Notes.Find(filter).FirstOrDefaultAsync();
            if (note.Result == null)
                return false;
            var update = Builders<Note>.Update
                .Set(x => x.Title, model.Title)
                .Set(x => x.Update, model.Update)
                .Set(x => x.Category, category);

            await _repository.Notes.UpdateOneAsync(filter, update);
            return true;
        }

        public async Task<DeleteResult> RemoveNote(string id, string userId)
        {
            var filter = Builders<Note>.Filter.And(
                Builders<Note>.Filter.Eq(n => n.Id, ObjectId.Parse(id)),
                Builders<Note>.Filter.Eq(n => n.UserId, ObjectId.Parse(userId))
            );
            return await _repository.Notes.DeleteOneAsync(filter);
        }
        
        public async Task<DeleteResult> RemoveAllNotes(string userId)
        {
            return await _repository.Notes.DeleteManyAsync(x => x.UserId == ObjectId.Parse(userId));
        }
    }
}