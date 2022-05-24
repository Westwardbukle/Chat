using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.Database.Model.Base
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}