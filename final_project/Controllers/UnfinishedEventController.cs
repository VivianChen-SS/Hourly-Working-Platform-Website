using final_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Diagnostics;
using System.Net.Mail;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mime;

namespace final_project.Controllers
{
    public class UnfinishedEventController : Controller
    {
    
        finalprojectEntities db = new finalprojectEntities();

        //private static int numberOfWorkers = 0;
        //private static int numberOfHired = 0;

        // GET: UnfinishedEvent
        public ActionResult Index(string sortOrder,string currentFilter, string searchstring,string clearSearchButton, int? page, bool? onePage)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.WorkDateSortParm = sortOrder == "workDate" ? "workDate_desc" : "workDate";
            ViewBag.OffListDateSortParm = sortOrder == "offListDate" ? "offListDate_desc" : "offListDate";
            ViewBag.heatSortParm = sortOrder == "heat" ? "heat_desc" : "heat";

            if (clearSearchButton != null)
            {
                searchstring = null;
            }
            else if (searchstring != null)
            {
                page = 1;
            }
            else
            {
                searchstring = currentFilter;
            }
            ViewBag.CurrentFilter = searchstring;

            string employerID = Session["EmployerID"].ToString();
            DateTime now = DateTime.Now;
            int nowHours = DateTime.Now.Hour;
            var dbWorks = db.Events
                .Where(e => e.EmployerID == employerID)
                .Where(e => e.OffListTime > now)
                .Where(e => e.LackofWorkers != 0);

            if (dbWorks.Count() == 0)
                return RedirectToAction("NoUnfinishedEvent");

            if (!String.IsNullOrEmpty(searchstring) && !(searchstring.Equals("尚未招滿") || searchstring.Equals("已招滿") || searchstring.Equals("全部") ))   
            {

                var selectedItems = dbWorks
                    .Where(s => s.Name.Contains(searchstring)
                        || s.EVENTTABs.Any(t => t.EventName.Contains(searchstring)));
                if (selectedItems.Count() != 0)
                {
                    dbWorks = selectedItems;
                }
                else
                {
                    ViewBag.selectedNothing = true;
                }
            }
            IQueryable<UnfinishedEventsIndexViewModels> works = initIndexViewModel(dbWorks);
            switch (searchstring)
            {
                case "尚未招滿": works = works.Where(w => w.StillNeed != 0);  break;
                case "已招滿": works = works.Where(w => w.StillNeed == 0); break;
                case "全部": break;
            }

            switch (sortOrder)
            {
                case "workDate":
                    works = works.OrderBy(d => d.Date).ThenBy(t => t.Time_Start); break;
                case "workDate_desc":
                    works = works.OrderByDescending(d => d.Date).ThenByDescending(t => t.Time_Start); break;
                case "offListDate":
                    works = works.OrderBy(d => d.OffListHoursLeft);break;
                case "offListDate_desc":
                    works = works.OrderByDescending(d => d.OffListHoursLeft); break;
                case "heat_desc":
                    works = works.OrderByDescending(d => d.HeatColorPercentage); break;
                case "heat":
                    works = works.OrderBy(d => d.HeatColorPercentage); break;

                default:  //workDate ascending
                    works = works.OrderBy(d => d.Date).ThenBy(t => t.Time_Start); break;
            }

            onePage = onePage ?? false;
            ViewBag.onePage = (bool)onePage ? false : true;
            int pageSize = (bool)onePage ? works.Count() : 8;
            int pageNumber = (page ?? 1);
            return View(works.ToPagedList(pageNumber,pageSize));

        }
        public ActionResult NoUnfinishedEvent()
        {
            return View();
        }
      
        [Route("UnfinishedEvent/EventPanel/{id}/{noApplier?}")]
        public ActionResult EventPanel(string id, bool? noApplier)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (id == null)
                return RedirectToAction("Index");
            EVENT work = db.Events.Where(e => e.EventID == id).Single();
            //numberOfWorkers = work.NumberOfWorkers;
            //numberOfHired = work.NumberOfWorkers;
            ViewBag.Id = id;
            ViewBag.Name = work.Name;
            ViewBag.NumberOfWorkers = work.NumberOfWorkers;
            ViewBag.LackofWorkers = work.LackofWorkers;
            ViewBag.NoApplier = noApplier == true ? true : false;
            return View();
        }



        public ActionResult Details(string id, bool? showHires, bool? noApplier)
        {
            //【注意！！！】因為寄信給打工者看工作條件內容，所以這邊刻意不加防呆

            EVENT work = db.Events.Where(e => e.EventID == id).Single();
            List<string> workerNameList = new List<string>();
            foreach (EVENT_WORKER_JUNCTION hire in db.Hires.Where(w => w.EventID == id))
            {
                if (hire.Hire)
                    workerNameList.Add(hire.WORKER.Name);
            }
            if (showHires != false)
            {
                ViewBag.WorkerNameList = workerNameList;
                ViewBag.NumberOfHired = work.NumberOfWorkers - work.LackofWorkers;
            }
            ViewBag.NoApplier = noApplier == true? true : false;

            return View(work);
        }

        public ActionResult ResumeList(string id,string sortOrder,string currentFilter,string SearchString, string clearSearch, int? page, bool? onePage)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (id == null)
                return RedirectToAction("Index");
            ViewBag.ID = id;
            ViewBag.CurrentSort = sortOrder; //記得sort資料「用什麼」排序
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AgeSortParm = sortOrder == "age" ? "age_desc" : "age";
            ViewBag.HiredSortParm = sortOrder == "hired" ? "hired_desc" : "hired";
            ViewBag.RatingSortParm = sortOrder == "rating" ? "rating_desc" : "rating";

            if (clearSearch != null)
            {
                SearchString = null;
            }
            else if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString; //記得搜尋內容
            
            EVENT work = db.Events.Where(w => w.EventID == id).Single();
            var applies = initResumeListViewModels(id);
            if (applies == null)
            {
                return RedirectToAction("EventPanel", new { id = work.EventID, noApplier = true });
            }
            if (!String.IsNullOrEmpty(SearchString))
            {
                var temp = applies.Where(e => e.Name.Contains(SearchString)
                                        || e.School.Contains(SearchString)
                                        || e.SkillTabs.Any(t => t.Contains(SearchString)) );
                if (temp.Count() == 0)
                {
                    ViewBag.SelectedNothing = "none";
                }
                else
                {
                    applies = temp;
                }
            }

            switch (sortOrder)
            {
                case "name_desc": applies = applies.OrderByDescending(a => a.Name); break;
                case "age": applies = applies.OrderByDescending(a => a.Age); break;
                case "age_desc": applies = applies.OrderBy(a => a.Age); break;
                case "hired": applies = applies.OrderBy(a => a.Hire); break;
                case "hired_desc": applies = applies.OrderByDescending(a => a.Hire); break;
                case "rating": applies = applies.OrderBy(a => a.Rating); break;
                case "rating_desc": applies = applies.OrderByDescending(a => a.Rating); break;
                default: applies = applies.OrderBy(a => a.Name); break;
            }
            onePage = onePage ?? false;
            ViewBag.onePage = (bool)onePage ? false : true ;
            int pageSize = (bool)onePage ? applies.Count() : 9;
            int pageNumber = (page ?? 1);

            ViewBag.WorkName = work.Name;
            ViewBag.NumberOfWorkers = work.NumberOfWorkers;
            ViewBag.LackofWorkers = work.LackofWorkers;
            return View(applies.ToPagedList(pageNumber,pageSize));
        }

        [Route("UnfinishedEvent/ApplierDetails/{id}/{workId}")]
        public ActionResult ApplierDetails(string id, string workId,bool? hire, int? rating)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (workId == null || id == null)
                return RedirectToAction("Index");

            WORKER applier = db.Workers.Where(w => w.WorkerID == id).SingleOrDefault();
            if (applier == null)
                return HttpNotFound();
            String empId = Session["EmployerID"].ToString();
            var emp = db.Employers.Where(e => e.EmployerID == empId).Single();
            ViewBag.EmpName = emp.Name;
            ViewBag.EmpTel = emp.Tel;
            ViewBag.EmpEmail = db.AspNetUsers.Where(i => i.Id == emp.Id).Single().Email;
            ViewBag.Id = workId;
            ViewBag.Hire = hire;
            if (rating != null) ViewBag.Rating = rating;
            ViewBag.Experience = getExperience(applier.EVENTs);

            EVENT work = db.Events.Where(e => e.EventID == workId).Single();
            ViewBag.NumberOfWorkers = work.NumberOfWorkers;
            ViewBag.LackofWorkers = work.LackofWorkers;
            return View(applier);
        }

        [Route("UnfinishedEvent/ApplierDetails/{id}/{workId}")]
        [HttpPost,ActionName("ApplierDetails")]
        [ValidateInput(false)]
        public ActionResult ApplierDetailsPost(string id, string workID, bool? hire,int? rating, string mailContext, string mailSubject)
        {
            //寄信
            if (Request.Form["sendMail"] != null)
            {
                WORKER applier = db.Workers
                 .Where(w => w.WorkerID == id)
                 .Single();
                String empId = Session["EmployerID"].ToString();
                EMPLOYER emp = db.Employers
                    .Where(e => e.EmployerID == empId)
                    .Single();
                var workLink = Url.Action("Details", "UnfinishedEvent", new { id = workID, showHires = false }, protocol: Request.Url.Scheme);

                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(applier.Email));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString(),"【東吳資管_109專題N組】人力共享平台");
                mail.Subject = mailSubject; //信件主旨
                mailContext = mailContext
                    + "<hr  style=\"height: 2px; margin: 0px 0px 30px 0px; color: gray; background - color:gray\"><br/>雇主姓名 :  " + emp.Name
                    + "<br/>雇主信箱:  " + db.AspNetUsers.Where(i => i.Id == emp.Id).Single().Email
                    + "<br/>雇主電話:  " + emp.Tel
                    + "<br/>請詳閱：<a href =\"" + workLink + "\">工作內容詳情</a>"
                    + "<br/>––––––發自【第N組專題__人力共享平台】的信。若有要事，請直接連絡雇主。請勿回信，系統無法幫您轉達🙂";
                //mail.Body = mailContext;
                mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(mailContext, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_40****@azure.com", "");
                smtpClient.Credentials = credentials;
                smtpClient.Send(mail);

                ViewBag.SendMailOK = "Successed";
                ViewBag.MailContext = mailContext;
                ViewBag.Id = workID;
                ViewBag.Hire = hire;
                if (rating != null) ViewBag.Rating = rating;
                ViewBag.Experience = getExperience(applier.EVENTs);

                EVENT work = db.Events.Where(e => e.EventID == workID).Single();
                ViewBag.NumberOfWorkers = work.NumberOfWorkers;
                ViewBag.LackofWorkers = work.LackofWorkers;
                return View(applier);
            }

            //錄用
            else
            {
                //這個寫法不是很好，但是worker table的 EVENT_WORKER_JUNCTION竟然是沒東西。先這樣寫擋一下
                EVENT_WORKER_JUNCTION employment =  db.Hires.Where(h => h.WorkerID == id && h.EventID == workID).Single();
                employment.Hire = true;
                employment.HireDateTime = DateTime.Now;
                db.Events.Where(e => e.EventID == workID).Single().LackofWorkers--;
                db.SaveChanges();
                return RedirectToAction("ResumeList", "UnfinishedEvent", new { id = workID });
            }
        }

        public ActionResult RetrieveImage(string id)
        {
            byte[] picture = GetImageFromDataBase(id);
            if (picture != null)
            {
                return File(picture, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        private byte[] GetImageFromDataBase(string workerID)
        {
            var picture = db.Workers.Find(workerID).ImageData;
            return picture;
        }

        private IQueryable<UnfinishedEventsIndexViewModels> initIndexViewModel(IQueryable<EVENT> dbWorks)
        {
            double[] percentageArray = calculateHeat(dbWorks);
            int count = 0;
            List<UnfinishedEventsIndexViewModels> unfinishedEvents = new List<UnfinishedEventsIndexViewModels>();
            foreach (var work in dbWorks)
            {
                UnfinishedEventsIndexViewModels temp = new UnfinishedEventsIndexViewModels {
                    EventID = work.EventID,
                    Name = work.Name,
                    Date = work.Date,
                    OfflistTime = work.OffListTime,
                    Time_Start = work.Time_Start,
                    Time_End = work.Time_End,
                    OffListHoursLeft = (int)(work.OffListTime - DateTime.Now).TotalHours,
                    WorkDayHoursLeft = (int)(work.Date.Add(work.Time_Start) - DateTime.Now).TotalHours,
                    NumberOfWorkers = work.NumberOfWorkers,
                    NumberOfHired = work.NumberOfWorkers - work.LackofWorkers,
                    StillNeed = work.LackofWorkers
                };
               
                temp.HeatColorPercentage = percentageArray[count];
                unfinishedEvents.Add(temp);
                count++;
            }

            return unfinishedEvents.AsQueryable();
        }

        private double[] calculateHeat(IQueryable<EVENT> works)
        {
            //STEP1: Cauculate Heat 熱度 = 尚缺人數 / 下架剩餘幾個四分之一天
            double[] heatArray = new double[works.Count()];
            double[] hiredArray = new double[works.Count()];
            double[] lackArray = new double[works.Count()];
            double[] remainingHoursArray = new double[works.Count()];
            int count = 0;
            foreach (var work in works)
            {
                //count how many people are hired
                int hiredCount = 0;
                foreach (EVENT_WORKER_JUNCTION junctionTable in work.EVENT_WORKER_JUNCTION)
                {
                    if (junctionTable.Hire)
                    {
                        hiredCount++;
                    }
                }
                int lackCount = work.NumberOfWorkers - hiredCount;

                //!!!!!!!!!!
                double total = (work.OffListTime - DateTime.Now).TotalHours / 6;
                double heat = (double)lackCount / (double)total;
                heatArray[count] = heat;
                hiredArray[count] = hiredCount;
                lackArray[count] = lackCount;
                remainingHoursArray[count] = total;
                count++;  //大一用了一整年的array，現在突然發現array真TM難用，但我還是懶得把Interger跟int之間切換
            }

            //STEP2: Cauculate all heats' standardDeviation
            //C# SD: https://stackoverflow.com/questions/5336457/how-to-calculate-a-standard-deviation-array
            double average = heatArray.Average();
            double sumOfSquaresOfDifferences = heatArray.Sum(val => (val - average) * (val - average));
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / heatArray.Length);
            //ViewBag.heatAverage = average;
            //ViewBag.heatSD = standardDeviation;

            //STEP3: Cauculate zScore for each work(event)
            //double[] zScoreArray = new double[heatArray.Length];
            double[] zScoreRoundArray = new double[heatArray.Length];
            for (int i = 0; i < zScoreRoundArray.Length; i++)
            {
                //zScoreArray[i] = (heatArray[i] - average) / standardDeviation;
                zScoreRoundArray[i] = Math.Round((heatArray[i] - average) / (standardDeviation * 0.5));
            }

            //STEP4: Convert Zscore into percentage for CSS to display color
            double zScoreRange = zScoreRoundArray.Max() - zScoreRoundArray.Min();
            double[] percentageArray = new double[zScoreRoundArray.Length];
            for (int i = 0; i < percentageArray.Length; i++)
            {
                percentageArray[i] = Math.Round((zScoreRoundArray[i] - zScoreRoundArray.Min()) * 70 / zScoreRange);
                percentageArray[i] = 100 - percentageArray[i];
            }
            return percentageArray;
        }

        private IEnumerable<ResumeListViewModels> initResumeListViewModels(string workId)
        {
            var applies = db.Hires.Where(e => e.EventID == workId);
            if (applies.Count() == 0)
            {
                return null;
            }
            List<ResumeListViewModels> viewModels = new List<ResumeListViewModels>();
            foreach (var apply in applies)
            {
                ResumeListViewModels viewModel = new ResumeListViewModels {
                    WorkerID = apply.WorkerID,
                    Name = apply.WORKER.Name,
                    Age = DateTime.Today.Year - apply.WORKER.Birthday.Year,
                    School = apply.WORKER.School,
                    GraduationStatus = apply.WORKER.GraduationStatus,
                    SkillTabs = getSkillTabNames(apply.WORKER.SKILLTABs),
                    Hire = apply.Hire,
                    //Experience = getExperience(apply.WORKER.EVENTs),
                    Rating = calculateRating(workId, apply.WORKER),
                    hasPhoto = apply.WORKER.ImageData != null ? true : false
                };
                viewModels.Add(viewModel);
                Debug.WriteLine(apply.WORKER.Name + calculateRating(workId, apply.WORKER));
            }

            viewModels = REcalculateRating(viewModels);
            return viewModels;
        }

        private List<string> getSkillTabNames(ICollection<SKILLTAB> skillTabs)
        {
            List<string> skillTabNames = new List<string>();
            foreach (SKILLTAB tab in skillTabs)
            {
                skillTabNames.Add(tab.SkillName);
            }
            return skillTabNames;
        }
        private Dictionary<string, string> getExperience(ICollection<EVENT> works)
        {
            Dictionary<string, string> experiences = new Dictionary<string, string>();
            foreach (EVENT work in works)
            {
                experiences[work.EventID] = work.Name;
            }
            return experiences;
        }

        private int calculateRating(string workId, WORKER worker)
        {
            int rating = 0;
            EVENT work = db.Events.Where(w => w.EventID == workId).Single();
            // calculate skilltab rating
            foreach (SKILLTAB tab in worker.SKILLTABs)
            {
                if (work.SKILLTABs.Contains(tab))
                    rating += 2;
            }

            // calculate MBIT rating
            if (work.I && worker.I == true) rating++;
            if (work.E && worker.E == true) rating++;
            if (work.N && worker.N == true) rating++;
            if (work.S && worker.S == true) rating++;
            if (work.T && worker.T == true) rating++;
            if (work.F && worker.F == true) rating++;
            if (work.P && worker.P == true) rating++;
            if (work.J && worker.J == true) rating++;
            return rating;
        }
      
        private List<ResumeListViewModels> REcalculateRating(List<ResumeListViewModels> viewModels)
        {
            //rrrrr又是一個算數學的ヽ(*。>Д<)o゜

            //把所有rating倒出來放在一個array，然後去array裡面找max & min
            int[] ratingArr = new int[viewModels.Count()];
            for (int i = 0; i < ratingArr.Length; i++)
            {
                ratingArr[i] = viewModels[i].Rating;
            }
           
            int max = ratingArr.Max();
            int min = ratingArr.Min();

            //重新計算每一個人的rating (把range限縮在0~10之間)
            foreach (ResumeListViewModels applier in viewModels)
            {
                applier.Rating = (int)Math.Round((10.0 * (applier.Rating - min) ) / (max - min), MidpointRounding.AwayFromZero);
            }
            return viewModels;
        }
    }
}
