using System.Collections.Generic;
using System.Threading.Tasks;
using notebook.Models;

namespace notebook.DataLayer.Abstracts
{
    public interface ICategoryService
    {
        Task<bool> AddCategory(Category category);
        Task<IEnumerable<Category>> GetAllCategories();
    }
}