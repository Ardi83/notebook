using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using notebook.DataLayer.Abstracts;
using notebook.Models;

namespace notebook.DataLayer
{
    public class CategoryService : ICategoryService
    {
        private readonly MongoRepository _repository = null;

        public CategoryService(IOptions<Settings> settings)
        {
            _repository = new MongoRepository(settings);
        }

        public async Task<bool> AddCategory(Category category)
        {
            var filter = Builders<Category>.Filter.Eq("Name", category.Name);
            var categoryCheck = _repository.Categories.Find(filter).FirstOrDefaultAsync();
            if (categoryCheck.Result != null)
                return false;
            await _repository.Categories.InsertOneAsync(category);
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _repository.Categories.Find(x => true).ToListAsync();
        }
    }
}