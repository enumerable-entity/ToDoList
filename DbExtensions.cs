using System.Collections.Generic;
using System.Linq;
using ToDoList.Models;

namespace ToDoList
{
    public static class DbExtensions
    {
        public static List<Category> GetUserCategories(this ToDoListDBContext context, User user)
        {
            return context.Categories.Where(x => x.UserId == user.Id).ToList();
        }

        public static void AddCategory (this ToDoListDBContext context, string title, User currentUser)
        {
            context.Categories.Add(new Category { Title = title, User = currentUser });
            context.SaveChanges();
            
        }

    }
}
