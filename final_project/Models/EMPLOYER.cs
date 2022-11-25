//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace final_project.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class EMPLOYER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EMPLOYER()
        {
            this.EVENTs = new HashSet<EVENT>();
        }

        [Key]
        [Required]
        public string EmployerID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "請勿輸入過長的文字字元")]
        public string Name { get; set; }

        [Required]
        [StringLength(5)]
        public string Gender { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "電話號碼格式不正確。(請勿輸入「-」或是空格，正確範例：0912345678。)")]
        [RegularExpression("([0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]*)", ErrorMessage = "電話號碼格式不正確。(請勿輸入「-」或是空格，正確範例：0912345678。)")]
        public string Tel { get; set; }

        [Required]
        [CheckValidBirthday]
        [DisplayFormat(DataFormatString = "{yyyy/M/d}")]
        public System.DateTime Birthday { get; set; }

        public string CompanyID { get; set; }

        [AllowHtml]
        public string Acknowledge { get; set; }

        [Required]
        public string Id { get; set; }
        public byte[] ImageData { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual COMPANY COMPANY { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENT> EVENTs { get; set; }
    }
}
