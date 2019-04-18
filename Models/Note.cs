using System;
using MongoDB.Bson;

namespace notebook.Models
{
    public class Note
    {
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public Category Category { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? Create { get; set; }
        public DateTime? Update { get; set; }
    }
}