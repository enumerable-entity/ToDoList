using System.Data.Entity;
using ToDoList.Models;

namespace ToDoList
{
    internal class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
        }
        public DbSet<ListItemm> ListItems { get; set; }
    }
}
