using MongoDB.Bson;

namespace notebook.Models
{
    public class Category
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}