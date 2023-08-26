using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MonopolyTest.Models.Base;

namespace MonopolyTest.Models
{
    public class Pallet : BaseModel
    {
        public virtual ICollection<Box> Boxes { get; set; }
        [Required]
        public override DateTime ProductionDate 
        {
            get
            {
                if(Boxes!=null && Boxes.Count>0)
                    // Наименьшая дата производства + 100 дней 
                    return Boxes.Min(p => p.ProductionDate)+ TimeSpan.FromDays(100);
                else
                    return DateTime.MinValue;////////////////
            }
        }
        public override double Weight
        {
            get
            {
                if (Boxes != null && Boxes.Count > 0)
                    return Boxes.Sum(p => p.Weight) + 30.0;
                else
                    return 30.0;
            }
        }
    }
}
