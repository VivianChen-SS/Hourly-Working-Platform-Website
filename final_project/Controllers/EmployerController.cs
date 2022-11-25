using final_project.Models;
using final_project.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity.Validation;

namespace final_project.Controllers
{
    public class EmployerController : Controller
    {
        finalprojectEntities db = new finalprojectEntities();
        // GET: Employer
        public ActionResult Index()
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            
            string employerID = Session["EmployerID"].ToString();
            EMPLOYER employer = db.Employers
                .Include(i => i.COMPANY)
                .Where(i => i.EmployerID == employerID)
                .SingleOrDefault();

            if (employer == null)
                return RedirectToAction("Login", "Account");
            
            return View(employer);
        }

        public ActionResult Edit(string id)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EMPLOYER employer = db.Employers
                .Include(i => i.COMPANY)
                .Where(i => i.EmployerID == id)
                .SingleOrDefault();

            //這一段照理說應該要寫，但是http error 404 的頁面沒有排，盡量讓它不要出現 
            //if (employer == null)
            //{
            //    return HttpNotFound();
            //}

            DropDownListRepository listItem = new DropDownListRepository();
            ViewBag.genderList = new SelectList(listItem.getGenderListItem(), "Text", "Value");
            ViewBag.industryList = new SelectList(listItem.getIndustryListItem(), "Text", "Value");
            return View(employer);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult SubmittedEdit(string clearCompany, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var instructorToUpdate = db.Employers
                .Include(i => i.COMPANY)
                .Where(i => i.EmployerID == id)
                .Single();

            if(TryUpdateModel(instructorToUpdate, "",
                new string[] { "Name", "PhotoRoute", "Gender", "Tel", "Birthday", "Acknowledge","COMPANY" }))
            {
                try
                {
                    //clearCompany傳進來的是要被clear的company的ID
                    if (clearCompany != null)
                    {
                        if (instructorToUpdate.COMPANY != null)
                        {
                            instructorToUpdate.COMPANY = null;
                        }

                        //加這一行，COMPANY資料表的那一筆才會刪掉
                        //如果以後要改搜尋，就註解調這一行，目前加這一行是因為不要讓company一直變超多
                        if (db.Companies.Find(clearCompany) != null)
                        {
                            db.Companies.Remove(db.Companies.Find(clearCompany));
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            DropDownListRepository listItem = new DropDownListRepository();
            ViewBag.genderList = new SelectList(listItem.getGenderListItem(), "Text", "Value");
            ViewBag.industryList = new SelectList(listItem.getIndustryListItem(), "Text", "Value");
            return View(instructorToUpdate);
        }

        public ActionResult CreateCompany(string id)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");

            EMPLOYER employer = db.Employers
                .Include(i => i.COMPANY)
                .Where(i => i.EmployerID == id)
                .SingleOrDefault();

            if (employer == null)
            {
                return HttpNotFound();
            }
              if (employer.COMPANY != null)
            {
                return RedirectToAction("Index");
            }

            //edit HasCompany view bag
            //PopulateHasCompanyData(employer);
            DropDownListRepository listItem = new DropDownListRepository();
            ViewBag.industryList = new SelectList(listItem.getIndustryListItem(), "Text", "Value");
            return View(employer);
        }

        [HttpPost,ActionName("CreateCompany")]
        public ActionResult SubmittedCreateCompany(string id, COMPANY company)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Employers
                .Include(i => i.COMPANY)
                .Where(i => i.EmployerID == id)
                .Single();
            
            if (TryUpdateModel(instructorToUpdate, "",
                new string[] {"COMPANY" }))
            {
                try
                {
                    instructorToUpdate.CompanyID = instructorToUpdate.COMPANY.CompanyID;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(instructorToUpdate);

        }

       
        public ActionResult UploadPicture(string id)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");

            ImageViewModels model = new ImageViewModels();
            EMPLOYER employer = db.Employers
                .Include(i => i.COMPANY)
                .Where(i => i.EmployerID == id)
                .SingleOrDefault();
            model.empID = employer.EmployerID;
            model.image = employer.ImageData;
            return View(model);
        }

        [HttpPost]
        public ActionResult UploadPicture(ImageViewModels imageViewModels)
        {
            HttpPostedFileBase file = Request.Files["PictureData"];
            ImageRepository service = new ImageRepository();

            EMPLOYER employer = db.Employers.Find(imageViewModels.empID);

            employer = service.UploadImageInDataBase(file, employer);
            if (ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(imageViewModels);
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

        public ActionResult AllWorksIndex(string sortOrder, string currentFilter, string searchstring, string clearSearchButton, int? page, bool? onePage)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.WorkDateSortParm = sortOrder == "workDate" ? "workDate_desc" : "workDate";
            ViewBag.NumberOfWorkers = sortOrder == "numberOfWorkers" ? "numberOfWorkers_desc" : "numberOfWorkers";
            ViewBag.EstimatedCost = sortOrder == "estimatedCost" ? "estimatedCost_desc" : "estimatedCost";
            ViewBag.ActualCost = sortOrder == "actualCost" ? "actualCost_desc" : "actualCost";

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
                .Where(e => e.EmployerID == employerID);

            if (dbWorks.Count() == 0)
                return RedirectToAction("NoUnfinishedEvent", "UnfinishedEvent");

            if (!String.IsNullOrEmpty(searchstring) &&
                    !(searchstring.Equals("徵人中")
                    || searchstring.Equals("已下架，但工作尚未開始")
                    || searchstring.Equals("工作中")
                    || searchstring.Equals("工作結束，未結清工資")
                    || searchstring.Equals("已結清工資")
                    || searchstring.Equals("全部")))
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
          
            IQueryable<AllEventsViewModels> works = initAllEventsViewModels(dbWorks);

            switch (searchstring)
            {
                case "徵人中": works = works.Where(e => e.statusColor == "#ffb3b3"); break;
                case "已下架，但工作尚未開始": works = works.Where(e => e.statusColor == "#ffb84d"); break;
                case "工作中": works = works.Where(e => e.statusColor == "#FFD700"); break;
                case "工作結束，未結清工資": works = works.Where(e => e.statusColor == "#9fdfbf"); break;
                case "已結清工資": works = works.Where(e => e.statusColor == "#dab3ff"); break;
                case "全部": break;
            }

            switch (sortOrder)
            {
                case "workDate":
                    works = works.OrderBy(d => d.Date).ThenBy(t => t.Time_Start); break;
                case "workDate_desc":
                    works = works.OrderByDescending(d => d.Date).ThenByDescending(t => t.Time_Start); break;
                case "numberOfWorkers":
                    works = works.OrderBy(d => d.NumberOfWorkers).ThenBy(t => t.Time_Start); break;
                case "numberOfWorkers_desc":
                    works = works.OrderByDescending(d => d.NumberOfWorkers).ThenByDescending(t => t.Time_Start); break;
                case "estimatedCost":
                    works = works.OrderBy(d => d.EstimatedCost).ThenBy(t => t.Time_Start); break;
                case "estimatedCost_desc":
                    works = works.OrderByDescending(d => d.EstimatedCost).ThenByDescending(t => t.Time_Start); break;
                case "actualCost":
                    works = works.OrderBy(d => d.ActualCost).ThenBy(t => t.Time_Start); break;
                case "actualCost_desc":
                    works = works.OrderByDescending(d => d.ActualCost).ThenByDescending(t => t.Time_Start); break;
               
                default:  //workDate ascending
                    works = works.OrderBy(d => d.Date).ThenBy(t => t.Time_Start); break;
            }

            onePage = onePage ?? false;
            ViewBag.onePage = (bool)onePage ? false : true;
            int pageSize = (bool)onePage ? works.Count() : 8;
            int pageNumber = (page ?? 1);
            return View(works.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult WorkDetails(string id)
        {
            if (Session["EmployerID"] == null)
                return RedirectToAction("Login", "Account");

            EVENT work = db.Events.Where(e => e.EventID == id).Single();
            List<string> workerNameList = new List<string>();
            bool finishable = true;
            foreach (var employee in work.EVENT_WORKER_JUNCTION)
            {
                if (employee.Hire)
                    workerNameList.Add(employee.WORKER.Name);
                if (!employee.Finished && employee.Hire)
                {
                    finishable = false;
                }
            }
            ViewBag.workerNameList = workerNameList;
            if (work.Finished)
            {
                ViewBag.ShowFinishButton = "已結清工資";
            }
            else if ((work.Date + work.Time_Start) > DateTime.Now)
            {
                ViewBag.ShowFinishButton = "工作尚未開始呢！(結清工資按紐，會在工作開始後出現。)";
            }
            else if (work.NumberOfWorkers == work.LackofWorkers)
            {
                ViewBag.ShowFinishButton = "沒有雇員，無須結清工資。";
            }
            else if (!finishable)
            {
                ViewBag.ShowFinishButton = "還有雇員尚未在app端確認已結清工資，請通知他們盡快確認";
            }
            else
            {
                ViewBag.ShowFinishButton = "OK";
            }
            return View(work);
        }

        [HttpPost, ActionName("WorkDetails")]
        public ActionResult FinishWork(string id)
        {
            if (ModelState.IsValid)
            {
                var work = db.Events.Where(e => e.EventID == id).Single().Finished = true;
                db.SaveChanges();
                return (RedirectToAction("WorkDetails"));
            }
            else
            {
                return View();
            }
        }

        private byte[] GetImageFromDataBase(string empID)
        {
            var picture = db.Employers.Find(empID).ImageData;
            return picture;
        }


        private IQueryable<AllEventsViewModels> initAllEventsViewModels(IQueryable<EVENT> dbWorks)
        {
            List<AllEventsViewModels> works = new List<AllEventsViewModels>();
            foreach (EVENT work in dbWorks)
            {
                int elapsedHours = -1;
                if (work.Time_End != null)
                {
                    //double = (datetime1 - dateTime2).TotalHours; 
                    elapsedHours = (int)Math.Round(
                        ((work.Date + (TimeSpan)work.Time_End) - (work.Date + work.Time_Start)).TotalHours);
                }

                AllEventsViewModels viewModels = new AllEventsViewModels {
                    EventID = work.EventID,
                    Name = work.Name,
                    Date = work.Date,
                    Time_Start = work.Time_Start,
                    Time_End = work.Time_End,
                    NumberOfWorkers = work.NumberOfWorkers,
                    EstimatedCost = calculateEstimatedCost(work, elapsedHours),
                    ActualCost = calculatetotalExpenses(work, elapsedHours),
                    statusColor = setStatusColor(work)
                };
                works.Add(viewModels);
            }
            return works.AsQueryable();
        }

        private string calculateEstimatedCost(EVENT work, int elapsedHours)
        {
            if (work.Wage_byHour == null)
                return "非以時計費";
            if (work.Time_End == null)
                return "無結束時間";

            return (int)work.Wage_byHour * work.NumberOfWorkers * elapsedHours + "";
        }

        private string calculatetotalExpenses(EVENT work, int elapsedHours)
        {
            if (work.Wage_byHour == null)
                return "非以時計費";
            if (work.Time_End == null)
                return "無結束時間";

            int hiredCount = db.Hires
                .Where(h => h.EventID == work.EventID && h.Hire)
                .Count();
            return (int)work.Wage_byHour * hiredCount * elapsedHours + "";
        }

        private string setStatusColor(EVENT work)
        {
            if (work.OffListTime > DateTime.Now) return "#ffb3b3";  //徵人中
            if (work.OffListTime < DateTime.Now && (work.Date + work.Time_Start) > DateTime.Now) return "#ffb84d";  //已下架，但工作尚未開始
            if (work.Time_End != null)
            {
                if ((work.Date + work.Time_Start) < DateTime.Now && (work.Date + work.Time_End) > DateTime.Now) return "#FFD700";  //工作中
                if ((work.Date + work.Time_End) < DateTime.Now && !work.Finished) return "#9fdfbf";  // 工作結束，未結清工資
            }
            else
            {
                if ((work.Date + work.Time_Start) < DateTime.Now && !work.Finished) return "#9fdfbf";  // 工作結束，未結清工資
            }
            if (work.Finished) return "#dab3ff";  //已結清工資
            else return "#ffffff";
        }
    }
}

