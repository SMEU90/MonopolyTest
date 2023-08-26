using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MonopolyTest.Models.Base
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Width { get; set; }
        [Required]
        public double Length { get; set; }
        [Required]
        public double Depth { get; set; }
        [Required]
        public virtual double Weight { get; set; }
        [Required]
        public virtual DateTime ProductionDate { get; set; }
        public double Volume
        {
            get
            {
                return Length * Depth * Depth;
            }
        }
    }
}
