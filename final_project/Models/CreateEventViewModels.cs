using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace final_project.Models
{
    public class CreateEventViewModels
    {
        [Required]
        public string EventID { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "請輸入正確字元")]
        public string Name { get; set; }

        [Required]
        [CheckValidDate]
        //[DisplayFormat(DataFormatString = "{yyyy/M/d}")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/M/d}")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time_Start { get; set; }

        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan? Time_End { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "電話號碼格式不正確。(請勿輸入「-」或是空格，正確範例：0912345678。)")]
        [RegularExpression("([0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]*)", ErrorMessage = "電話號碼格式不正確。(請勿輸入「-」或是空格，正確範例：0912345678。)")]
        public string Tel { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "請勿輸入過長的文字敘述")]
        public string Address { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "人數不可少於0")]
        public int NumberOfWorkers { get; set; }

        [Required]
        public string EmployerID { get; set; }

        [Range(160, int.MaxValue, ErrorMessage = "最低時薪160NT$/hr。")]
        public int? Wage_byHour { get; set; }

        [StringLength(300, ErrorMessage = "請輸入薪資計算方式以及金額")]
        public string Wage_byOthers { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "輸入數字不應低於應徵人數")]
        public int? maxResumeNumber { get; set; }

        [AllowHtml]
        public string Abouts { get; set; }

        [CheckValidDate]
        [Required(ErrorMessage = "請輸入同時符合早於工作日且晚於填寫當下時刻之時間")]
        [DisplayFormat(DataFormatString = "{yyyy/M/d}")]
        public DateTime OffListTime { get; set; }


        //不能叫event因為是保留字QAQ
        //public EVENT work { get; set; }
        public ICollection<EVENTTAB> workTabs { get; set; }
        public ICollection<SKILLTAB> skillTabs { get; set; }
    }
}