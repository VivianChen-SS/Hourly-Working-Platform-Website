using final_project.Models;
using final_project.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace final_project.Controllers
{
    public class CreateEventController : Controller
    {
        finalprojectEntities db = new finalprojectEntities();
        ChosenTabsRepository chosenTabs = new ChosenTabsRepository();
        MBITPreferenceRepository MBITs = new MBITPreferenceRepository(); //起名無能中......
        private static EVENT workToAdd;

        // GET: CreateEvent/Create
        public ActionResult Create()
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");

            string employerID = Session["EmployerID"].ToString();
            CreateEventViewModels viewModels = new CreateEventViewModels();
            viewModels.EmployerID = employerID;
            if (workToAdd == null)
            {
                chosenTabs.clearSkillTabRepository();
                chosenTabs.clearWorkTabRepository();
                chosenTabs.clearRequiredSkillTabIDList();
                MBITs.clearMBITPreferenceRepository();
            }
            else
            {
                viewModels.EventID = workToAdd.EventID;
                viewModels.Name = workToAdd.Name;
                viewModels.Date = workToAdd.Date;
                viewModels.Time_Start = workToAdd.Time_Start;
                viewModels.Time_End = workToAdd.Time_End;
                viewModels.Tel = workToAdd.Tel;
                viewModels.Address = workToAdd.Address;
                viewModels.NumberOfWorkers = workToAdd.NumberOfWorkers;
                viewModels.Wage_byHour = workToAdd.Wage_byHour;
                viewModels.Wage_byOthers = workToAdd.Wage_byOthers;
                viewModels.maxResumeNumber = workToAdd.maxResumeNumber;
                viewModels.Abouts = workToAdd.Abouts;
                viewModels.OffListTime = workToAdd.OffListTime;
            }
            return View(viewModels);
        }
        

        [HttpPost]
        public ActionResult Create(CreateEventViewModels viewModels,string showTimeEndCheckbox,string byHourCheckbox)
        {
            //整個資料送出
            if (Request.Form["formSubmitButton"] != null)
            {
                //檢查一些輸入欄位之間的Validation
                bool invalid = false;
                if (viewModels.maxResumeNumber != null)
                {
                    if (viewModels.maxResumeNumber < viewModels.NumberOfWorkers)
                    {
                        ViewBag.InvalidResumeNumber = "⚠ 你收的履歷上限，小於應徵人數，這樣人數永遠招不滿喔！";
                        invalid = true;
                    }
                }
               
                if (viewModels.Time_End != null && showTimeEndCheckbox != null)
                {
                    if (String.Compare(viewModels.Time_End.ToString(), viewModels.Time_Start.ToString()) <= 0)
                    {
                        ViewBag.InvalidTimeSpan = "⚠ 結束時間在開始時間之前？？如果你真的發明了時光機，請直接找開發同學，她幫你開特例！";
                        invalid = true;
                    }
                }
                if (viewModels.Date.Add(viewModels.Time_Start) < viewModels.OffListTime)
                {
                    ViewBag.InvalidOffListTime = "⚠ 下架時間比工作開始時間晚，這樣不行喔~~";
                    invalid = true;
                }
                // if (viewModels.Date.Add(viewModels.Time_Start) < DateTime.Now || viewModels.OffListTime < DateTime.Now)
                //{
                //    ViewBag.InvalidOffListTime = "⚠ 請輸入未來日期。有能力穿越的人，都已經回古代改寫歷史了。";
                //    invalid = true;
                //}
                if (viewModels.Wage_byHour == null && viewModels.Wage_byOthers == null)
                {
                    ViewBag.InvalidWage = "⚠ 不給工資，真黑心............還是你忘了寫？？";
                    invalid = true;
                }
                if (invalid)
                {
                    return View(viewModels);
                }
                if (ModelState.IsValid)
                {
                    var eventToAdd = new EVENT
                    {
                        EventID = viewModels.EventID,
                        Name = viewModels.Name,
                        Date = viewModels.Date.Date,
                        Time_Start = viewModels.Time_Start,
                        Time_End = showTimeEndCheckbox == null ? null : viewModels.Time_End,
                        Tel = viewModels.Tel,
                        Address = viewModels.Address,
                        NumberOfWorkers = viewModels.NumberOfWorkers,
                        LackofWorkers = viewModels.NumberOfWorkers,
                        EmployerID = viewModels.EmployerID,
                        Wage_byHour = byHourCheckbox == null ? null : viewModels.Wage_byHour,
                        Wage_byOthers = byHourCheckbox == null ? viewModels.Wage_byOthers : null,
                        maxResumeNumber = viewModels.maxResumeNumber,
                        Abouts = viewModels.Abouts,
                        OffListTime = viewModels.OffListTime,
                        Finished = false
                    };
                    eventToAdd.SKILLTABs = new List<SKILLTAB>();
                    eventToAdd.EVENTTABs = new List<EVENTTAB>();
                    workToAdd = eventToAdd;
                    //db.Events.Add(eventToAdd);
                    //db.SaveChanges();
                    //return RedirectToAction("Index", "Home");

                    return RedirectToAction("ChooseTab");
                }
                return View(viewModels);
            }
            return View(viewModels);
        }


        public ActionResult ChooseTab()
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (workToAdd == null)
                return RedirectToAction("Create");
            initDisplayChosenTabs();
            EventChooseTabViewModel viewModels = new EventChooseTabViewModel();
            return View(viewModels);
        }

        [HttpPost]
        public ActionResult ChooseTab(EventChooseTabViewModel viewModels,List<string> selectedSkills, List<string> selectedWorks, List<string> selectedRequiredSkills)
        {
            chosenTabs.updateRequiredSkillTabIDList(selectedRequiredSkills);
            #region workTab(eventTab) region
            //按下搜尋按鈕
            if (Request.Form["workSearchButton"] != null && !String.IsNullOrEmpty(viewModels.WorkTabSearchString))
            {
                PopulateSearchedWorkTabs(viewModels.WorkTabSearchString);
                initDisplayChosenTabs();
                return View(viewModels);
            }

            //確認選擇已勾選標籤
            ViewBag.SearchedWorkTabs = null;
            if (Request.Form["workSelectButton"] != null)
            {
                chosenTabs.updateWorkTabIDList(selectedWorks);
                initDisplayChosenTabs();
                return View(viewModels);
            }
            //新增新標籤
            if (Request.Form["createWorkTabButton"] != null)
            {
                EVENTTAB evTab = new EVENTTAB
                {
                    EventTabID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    EventName = chosenTabs.getWorkTabSearchString()
                };
                if (ModelState.IsValid)
                {
                    db.EventTabs.Add(evTab);
                    db.SaveChanges();
                }
                chosenTabs.updateWorkTabIDList(evTab.EventTabID);
                initDisplayChosenTabs();
                return View(viewModels);
            }
            //取消新增新標籤
            if (Request.Form["notCreateWorkTabButton"] != null)
            {
                initDisplayChosenTabs();
                return View(viewModels);
            }
            #endregion

            #region skillTab region
            //按下搜尋按鈕
            if (Request.Form["skillSearchButton"] != null && !String.IsNullOrEmpty(viewModels.SkillTabSearchString))
            {
                PopulateSearchedSkillTabs(viewModels.SkillTabSearchString);
                initDisplayChosenTabs();
                return View(viewModels);
            }

            //確認選擇已勾選標籤
            ViewBag.SearchedSkillTabs = null;
            if (Request.Form["skillSelectButton"] != null)
            {
                chosenTabs.updateSkillTabIDList(selectedSkills);
                initDisplayChosenTabs();
                return View(viewModels);
            }

            //新增新標籤
            if (Request.Form["createSkillTabButton"] != null)
            {
                SKILLTAB skTab = new SKILLTAB
                {
                    SkillTabID = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                    SkillName = chosenTabs.getSkillTabSearchString()
                };
                if (ModelState.IsValid)
                {
                    db.SkillTabs.Add(skTab);
                    db.SaveChanges();
                }
                chosenTabs.updateSkillTabIDList(skTab.SkillTabID);
                initDisplayChosenTabs();
                return View(viewModels);
            }

            //取消新增新標籤
            if (Request.Form["notCreateSkillTabButton"] != null)
            {
                initDisplayChosenTabs();
                return View(viewModels);
            }

            #endregion

            if (Request.Form["formSubmitButton"] != null)
            {
                if (ModelState.IsValid)
                {
                    //workToAdd.SKILLTAB = new List<SKILLTAB>();
                    //foreach (string skillTabID in chosenTabs.getSkillTabIDList())
                    //{
                    //   workToAdd.SKILLTAB.Add(db.SkillTabs.Find(skillTabID));
                    //}
                    //workToAdd.EVENTTAB = new List<EVENTTAB>();
                    //foreach (string workTabID in chosenTabs.getWorkTabIDList())
                    //{
                    //   workToAdd.EVENTTAB.Add(db.EventTabs.Find(workTabID));
                    //}
                    //db.Events.Add(workToAdd);
                    //db.SaveChanges();
                    //return RedirectToAction("Index", "Home");

                    //return RedirectToAction("ComfirmCreateWork");
                    return RedirectToAction("MBITPreference");
                }
                initDisplayChosenTabs();
                return View(viewModels);
            }
            initDisplayChosenTabs();
            return View(viewModels);
        }


        public ActionResult MBITPreference()
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (workToAdd == null)
                return RedirectToAction("Create");

            MBITPreferenceViewModels viewModels = MBITs.getMBITFormAnswers() == null? new MBITPreferenceViewModels() : MBITs.getMBITFormAnswers();
            return View(viewModels);
        }
        [HttpPost]
        public ActionResult MBITPreference(MBITPreferenceViewModels viewModels)
        {
            if (ModelState.IsValid)
            {
                MBITs.updateAllContext(viewModels);
                MBITs.updateMBITFormAnswers(viewModels);
                return RedirectToAction("ComfirmCreateWork");
            }
            return View(viewModels);
        }

        public ActionResult ComfirmCreateWork()
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (workToAdd == null)
                return RedirectToAction("Create");

            initDisplayChosenTabs();
            initDisplayMBITPreference();
            return View(workToAdd);
        }

        [HttpPost, ActionName("ComfirmCreateWork")]
        public ActionResult Comfirm()
        {
            if (Request.Form["EditButton"] != null)
            {
                return RedirectToAction("Create");
            }
            if (Request.Form["formSubmitButton"] != null)
            {
                if (ModelState.IsValid)
                {
                    //save tabs
                    if (chosenTabs.getSkillTabIDList() != null)
                    {
                        workToAdd.SKILLTABs = new List<SKILLTAB>();
                        foreach (string skillTabID in chosenTabs.getSkillTabIDList())
                        {
                            workToAdd.SKILLTABs.Add(db.SkillTabs.Find(skillTabID));
                        }
                    }
                    if (chosenTabs.getWorkTabIDList() != null)
                    {
                        workToAdd.EVENTTABs = new List<EVENTTAB>();
                        foreach (string workTabID in chosenTabs.getWorkTabIDList())
                        {
                            workToAdd.EVENTTABs.Add(db.EventTabs.Find(workTabID));
                        }
                    }
                    if (chosenTabs.getRequiredSkillTabIDList() != null)
                    {
                        foreach (string requiredSkillTabID in chosenTabs.getRequiredSkillTabIDList())
                        {
                            workToAdd.REQUIREDSKILLTABs.Add(db.RequiredSkillTabs.Find(requiredSkillTabID));
                        }
                    }
                    

                    //save MBITPreferences
                    Dictionary<string, bool> MBITToAdd = MBITs.getMBITPreferences();
                    workToAdd.I = MBITToAdd["I"];
                    workToAdd.E = MBITToAdd["E"];
                    workToAdd.N = MBITToAdd["N"];
                    workToAdd.S = MBITToAdd["S"];
                    workToAdd.T = MBITToAdd["T"];
                    workToAdd.F = MBITToAdd["F"];
                    workToAdd.P = MBITToAdd["P"];
                    workToAdd.J = MBITToAdd["J"];
                    workToAdd.RequiredCharacter = MBITs.getRequiredChatacter();

                    db.Events.Add(workToAdd);
                    db.SaveChanges();
                    workToAdd = null;
                    return RedirectToAction("CreatedSuccessfully");
                }
            }
            return View(workToAdd);
        }

        public ActionResult CreatedSuccessfully()
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        private void initDisplayChosenTabs()
        {
            List<string> chosenSkillTabsNameList = new List<string>();
            foreach (string skillTabID in chosenTabs.getSkillTabIDList())
            {
                chosenSkillTabsNameList.Add( db.SkillTabs.Find(skillTabID).SkillName );
            }
            ViewBag.chosenSkillTabs = chosenSkillTabsNameList;

            List<string> chosenWorkTabsNameList = new List<string>();
            foreach (string workTabID in chosenTabs.getWorkTabIDList())
            {
                chosenWorkTabsNameList.Add( db.EventTabs.Find(workTabID).EventName );
            }
            ViewBag.chosenWorkTabs = chosenWorkTabsNameList;


            //以下這段if else寫得很爛，但是我想要休息了
            var requiredSkillsVM = new List<AssignedTabData>();
            var temp = chosenTabs.getRequiredSkillTabIDList();
            if (temp != null)
            {
                foreach (var tab in db.RequiredSkillTabs.ToList())
                {
                    requiredSkillsVM.Add(new AssignedTabData
                    {
                        TabID = tab.RequiredSkillTabID,
                        TabName = tab.RequiredSkillName,
                        Assigned = chosenTabs.getRequiredSkillTabIDList().Contains(tab.RequiredSkillTabID) ? true : false
                    });
                }
            }
            else
            {
                foreach (var tab in db.RequiredSkillTabs.ToList())
                {
                    requiredSkillsVM.Add(new AssignedTabData
                    {
                        TabID = tab.RequiredSkillTabID,
                        TabName = tab.RequiredSkillName,
                        Assigned = false
                    });
                }
            }
            ViewBag.RequiredSkills = requiredSkillsVM;

        }


        private void initDisplayMBITPreference()
        {
            Dictionary<string, bool> MBIT = MBITs.getMBITPreferences();
            List<string> MBITChineseDescriptions = new List<string>();
            foreach (KeyValuePair<string,bool> kpv in MBIT)
            {
                switch (kpv.Key)
                {
                    case "I": if(kpv.Value) MBITChineseDescriptions.Add("內向型"); break;
                    case "E": if (kpv.Value) MBITChineseDescriptions.Add("外向型"); break;
                    case "N": if (kpv.Value) MBITChineseDescriptions.Add("直覺型"); break;
                    case "S": if (kpv.Value) MBITChineseDescriptions.Add("實感型(辨識型)"); break;
                    case "T": if (kpv.Value) MBITChineseDescriptions.Add("思考型(理性)"); break;
                    case "F": if (kpv.Value) MBITChineseDescriptions.Add("情感型(感性)"); break;
                    case "J": if (kpv.Value) MBITChineseDescriptions.Add("判斷型(果斷型)"); break;
                    case "P": if (kpv.Value) MBITChineseDescriptions.Add("感知型(熟思型)"); break;
                }
            }
            ViewBag.MBITChineseDescriptions = MBITChineseDescriptions;
            string requiredCharacter = "";
            switch (MBITs.getRequiredChatacter())
            {
                case "I": requiredCharacter = "內向型"; break;
                case "E": requiredCharacter = "外向型"; break;
                case "N": requiredCharacter = "直覺型"; break;
                case "S": requiredCharacter = "實感型(辨識型)"; break;
                case "T": requiredCharacter = "思考型(理性)"; break;
                case "F": requiredCharacter = "情感型(感性)"; break;
                case "J": requiredCharacter = "判斷型(果斷型)"; break;
                case "P": requiredCharacter = "感知型(熟思型)"; break;
                case "NONE": requiredCharacter = "沒有特別指定"; break;
            }
            ViewBag.RequiredCharacter = requiredCharacter;
        }

        private void PopulateSearchedSkillTabs(string SkillTabSearchString)
        {
            var skillsRequired = db.SkillTabs.Where(s => s.SkillName.Contains(SkillTabSearchString));
            var sameTabs = db.SkillTabs.Where(s => s.SkillName.Equals(SkillTabSearchString));

            //找不到、沒資料 or 沒有一樣的 都給我加上新增標籤
            if (skillsRequired.Count() == 0 || sameTabs.Count() == 0 )
            {
                chosenTabs.setSkillTabSearchString(SkillTabSearchString);
                ViewBag.newSkillTab = SkillTabSearchString;

                //找不到、沒資料 就滾回去吧
                if (skillsRequired.Count() == 0)
                {
                    return;
                }
            }
            List<string> searchList = new List<string>();
            List<string> skillTabIDList = chosenTabs.getSkillTabIDList();
            var searchedTabs = new List<AssignedTabData>();
            foreach (var tab in skillsRequired)
            {
                searchedTabs.Add(new AssignedTabData
                {
                    TabID = tab.SkillTabID,
                    TabName = tab.SkillName,
                    Assigned = skillTabIDList.Contains(tab.SkillTabID) ? true : false
                });
                searchList.Add(tab.SkillTabID);
            }
            ViewBag.SearchedSkillTabs = searchedTabs;
            chosenTabs.setskillTabIDSearchList(searchList);
        }
        
           private void PopulateSearchedWorkTabs(string WorkTabSearchString)
        {
            var worksRequired = db.EventTabs.Where(s => s.EventName.Contains(WorkTabSearchString));
            var sameTabs = db.EventTabs.Where(s => s.EventName.Equals(WorkTabSearchString));

            //找不到、沒資料 or 沒有一樣的 都給我加上新增標籤
            if (worksRequired.Count() == 0 || sameTabs.Count() == 0 )
            {
                chosenTabs.setWorkTabSearchString(WorkTabSearchString);
                ViewBag.newWorkTab = WorkTabSearchString;

                //找不到、沒資料 就滾回去吧
                if (worksRequired.Count() == 0)
                {
                    return;
                }
            }
            List<string> searchList = new List<string>();
            List<string> workTabIDList = chosenTabs.getWorkTabIDList();
            var searchedTabs = new List<AssignedTabData>();
            foreach (var tab in worksRequired)
            {
                searchedTabs.Add(new AssignedTabData
                {
                    TabID = tab.EventTabID,
                    TabName = tab.EventName,
                    Assigned = workTabIDList.Contains(tab.EventTabID) ? true : false
                });
                searchList.Add(tab.EventTabID);
            }
            ViewBag.SearchedWorkTabs = searchedTabs;
            chosenTabs.setworkTabIDSearchList(searchList);
        }

       
    }
}