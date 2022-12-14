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

    public partial class WORKER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WORKER()
        {
            this.EVENT_WORKER_JUNCTION = new HashSet<EVENT_WORKER_JUNCTION>();
            this.EVENTTABs = new HashSet<EVENTTAB>();
            this.SKILLTABs = new HashSet<SKILLTAB>();
            this.EVENTs = new HashSet<EVENT>();
            this.REQUIREDSKILLTABs = new HashSet<REQUIREDSKILLTAB>();
        }

        [Required]
        public string WorkerID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "請輸入正確的字元")]
        public string Name { get; set; }

        [Required]
        public string PhotoRoute { get; set; }

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

        [StringLength(300)]
        public string School { get; set; }

        [StringLength(100)]
        public string GraduationStatus { get; set; }

        [Required]
        public string SelfInfo { get; set; }
        public string ProjectName_1 { get; set; }
        public string ProjectLink_1 { get; set; }
        public string ProjectName_2 { get; set; }
        public string ProjectLink_2 { get; set; }
        public string ProjectDescription { get; set; }
        public string SkillTabID { get; set; }
        public string WorkExperience { get; set; }
        public byte[] ImageData { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public Nullable<bool> I { get; set; }
        public Nullable<bool> E { get; set; }
        public Nullable<bool> N { get; set; }
        public Nullable<bool> S { get; set; }
        public Nullable<bool> T { get; set; }
        public Nullable<bool> F { get; set; }
        public Nullable<bool> P { get; set; }
        public Nullable<bool> J { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENT_WORKER_JUNCTION> EVENT_WORKER_JUNCTION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENTTAB> EVENTTABs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SKILLTAB> SKILLTABs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EVENT> EVENTs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<REQUIREDSKILLTAB> REQUIREDSKILLTABs { get; set; }
    }
}
