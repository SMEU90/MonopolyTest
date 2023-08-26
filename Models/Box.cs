using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MonopolyTest.Models.Base;

namespace MonopolyTest.Models
{
    public class Box : BaseModel
    {
        public int? PalletId { get; set; }
        [ForeignKey("PalletId")]
#nullable enable
        public virtual Pallet? Pallet { get; set; }
#nullable disable
    }
}
