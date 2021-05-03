using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Models
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
