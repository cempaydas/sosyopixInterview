using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PhotoBoom.Models;

    public class PhotoBoomContext : DbContext
    {
        public PhotoBoomContext (DbContextOptions<PhotoBoomContext> options)
            : base(options)
        {
        }

        public DbSet<PhotoBoom.Models.Gallery> Gallery { get; set; }
    }
