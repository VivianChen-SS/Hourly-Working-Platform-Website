using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace final_project.Repositories
{
    public class DropDownListRepository
    {
        public List<SelectListItem> getGenderListItem()
        {
            List<SelectListItem> genderListItem = new List<SelectListItem>();
            genderListItem.Add(new SelectListItem() { Text = "男", Value = "男" });
            genderListItem.Add(new SelectListItem() { Text = "女", Value = "女" });
            genderListItem.Add(new SelectListItem() { Text = "其他", Value = "其他" });
            genderListItem.Add(new SelectListItem() { Text = "拒答", Value = "拒答" });
            return genderListItem;
        }

        public List<SelectListItem> getIndustryListItem()
        {
            //資料來源：中華民國統計資訊網__第10次修訂-行業分類查詢 https://www.stat.gov.tw/ct.asp?mp=4&xItem=42276&ctNode=1309
            List<SelectListItem> IndustryListItem = new List<SelectListItem>();
            IndustryListItem.Add(new SelectListItem() { Text = "農、林、漁、牧業", Value = "農、林、漁、牧業" });
            IndustryListItem.Add(new SelectListItem() { Text = "礦業及土石採取業", Value = "礦業及土石採取業" });
            IndustryListItem.Add(new SelectListItem() { Text = "電力及燃氣供應業", Value = "電力及燃氣供應業" });
            IndustryListItem.Add(new SelectListItem() { Text = "用水供應及污染整治業", Value = "用水供應及污染整治業" });
            IndustryListItem.Add(new SelectListItem() { Text = "營建工程業", Value = "營建工程業" });
            IndustryListItem.Add(new SelectListItem() { Text = "批發及零售業", Value = "批發及零售業" });
            IndustryListItem.Add(new SelectListItem() { Text = "運輸及倉儲業", Value = "運輸及倉儲業" });
            IndustryListItem.Add(new SelectListItem() { Text = "住宿及餐飲業", Value = "住宿及餐飲業" });
            IndustryListItem.Add(new SelectListItem() { Text = "出版、影音製作、傳播及資通訊服務業", Value = "出版、影音製作、傳播及資通訊服務業" });
            IndustryListItem.Add(new SelectListItem() { Text = "金融及保險業", Value = "金融及保險業" });
            IndustryListItem.Add(new SelectListItem() { Text = "不動產業", Value = "不動產業" });
            IndustryListItem.Add(new SelectListItem() { Text = "專業、科學及技術服務業", Value = "專業、科學及技術服務業" });
            IndustryListItem.Add(new SelectListItem() { Text = "支援服務業", Value = "支援服務業" });
            IndustryListItem.Add(new SelectListItem() { Text = "公共行政及國防；強制性社會安全", Value = "公共行政及國防；強制性社會安全" });
            IndustryListItem.Add(new SelectListItem() { Text = "教育業", Value = "教育業" });
            IndustryListItem.Add(new SelectListItem() { Text = "醫療保健及社會工作服務業", Value = "醫療保健及社會工作服務業" });
            IndustryListItem.Add(new SelectListItem() { Text = "藝術、娛樂及休閒服務業", Value = "藝術、娛樂及休閒服務業" });
            IndustryListItem.Add(new SelectListItem() { Text = "其他服務業", Value = "其他服務業" });
            return IndustryListItem;
        }
    }
}