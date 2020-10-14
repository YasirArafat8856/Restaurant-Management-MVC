using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Restaurant.ViewModel
{
    public class ItemVm
    {
        //[Key]
        public int Id { get; set; }
        public string Item_Name { get; set; }
        public Nullable<decimal> Unit_Price { get; set; }
        public string Picture { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImgFile { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Food_Order> Food_Order { get; set; }
    }
}