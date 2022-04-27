using System.Collections.Generic;
using System.Linq;
using ToDoList.Models;

namespace ToDoList
{
    public static class UserExtensions
    {
        public static List<Category> GetUserCategories(this ToDoListContext context, int userId)
        {
            return context.Categories.Where(x => x.UserId == 1).ToList();
        }
    }
}
