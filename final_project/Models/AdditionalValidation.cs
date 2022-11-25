using System;
using System.ComponentModel.DataAnnotations;


namespace final_project.Models
{
    public class CheckValidBirthday : ValidationAttribute
    {
        public CheckValidBirthday()
        {
            //	勞動基準法：https://law.moj.gov.tw/LawClass/LawParaDeatil.aspx?pcode=N0030001&bp=5
            //到底要不要接受童工？？？
            ErrorMessage = "年齡不符。依據「勞動基準法」，最低工作年齡為15歲。";
        }

        public override bool IsValid(object value)
        {
            if(value != null)
            {
                DateTime birthday = DateTime.Parse(value.ToString());
                return birthday.AddYears(15) < DateTime.Now ? true : false;
            }
            return true;
        }
    }

    public class CheckValidDate : ValidationAttribute
    {
        public CheckValidDate()
        {
            ErrorMessage = "請輸入未來日期。有能力穿越的人，都已經回古代改寫歷史了。";
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                DateTime date = DateTime.Parse(value.ToString());
                return date > DateTime.Now ? true : false;
            }
            return true;
        }
    }
}