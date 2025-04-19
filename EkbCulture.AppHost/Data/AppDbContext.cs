﻿using k8s.KubeConfigModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EkbCulture.AppHost.Models;

namespace EkbCulture.AppHost.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
    }
}