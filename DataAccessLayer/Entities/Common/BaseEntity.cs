﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities.Common
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set;}

        public DateTime? DeletedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
