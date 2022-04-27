﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<ItemList> Lists { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }
    }
}
