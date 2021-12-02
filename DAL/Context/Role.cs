﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<User> Users { get; set; }
    }
}