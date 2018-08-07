using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TodoNotes.Models
{
    public class TodoNotesContext : DbContext
    {
        public TodoNotesContext (DbContextOptions<TodoNotesContext> options)
            : base(options)
        {
        }

        public DbSet<TodoNotes.Models.Notes> Notes { get; set; }
    }
}
