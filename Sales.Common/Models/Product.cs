﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sales.Common.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        [Display(Name = "Image")]
        public string ImagePath{ get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Is Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Publish On")]
        [DataType(DataType.Date)]
        public DateTime PublshOn { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public string ImageFullPath {
            get
            {
                if (String.IsNullOrEmpty(this.ImagePath))
                {
                    return "noproduct";
                    //return null;
                }

                //return $"https://salesbackend.azurewebsites.net{this.ImagePath.Substring(1)}";
                return $"https://salesapisevices.azurewebsites.net{this.ImagePath.Substring(1)}";
            }
        }
        
        public override string ToString()
        {
            return this.Description;
        }
    }
}
