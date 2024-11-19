//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using EasyPark.Data;
//using EasyPark.Models;
//using Microsoft.EntityFrameworkCore;

//namespace EasyPark.Controllers
//{
//    [Authorize]
//    public class SurveyController : Controller
//    {
//        private readonly IEmailSender _emailSender;
//        private readonly EasyParkContext _dbContext;

//        public SurveyController(IEmailSender emailSender, EasyParkContext dbContext)
//        {
//            _emailSender = emailSender;
//            _dbContext = dbContext;
//        }
//        // 顯示所有問卷
//        public IActionResult Index(string searchString, string filterStatus)
//        {
//            var surveys = from s in _dbContext.Survey select s;

//            if (!string.IsNullOrEmpty(searchString))
//            {
//                //surveys = surveys.Where(s => s.Email.Contains(searchString));
//                surveys = surveys.Include(s => s.User).Where(s => s.User.Email.Contains(searchString));
//            }

//            if (!string.IsNullOrEmpty(filterStatus))
//            {
//                if (filterStatus == "已回覆")
//                {
//                    surveys = surveys.Where(s => s.IsReplied);
//                }
//                else if (filterStatus == "未回覆")
//                {
//                    surveys = surveys.Where(s => !s.IsReplied);
//                }
//            }

//            return View(surveys.ToList());
//        }

//        // POST: Survey/Reply
//        [HttpPost]
//        public async Task<IActionResult> Reply(int id, string replyMessage)
//        {
//            var survey = _dbContext.Survey.Include(s=>s.User).FirstOrDefault(s => s.Id == id);
//            if (survey == null)
//            {
//                return NotFound();
//            }

//            // 更新問卷回覆信息
//            survey.ReplyMessage = replyMessage;
//            survey.IsReplied = true;
//            survey.RepliedAt = DateTime.Now;
//            survey.Status = "已回覆";

//            // 保存到資料庫
//            _dbContext.SaveChanges();

//            // 發送郵件給使用者
//            await _emailSender.SendEmailAsync(survey.User.Email, "Survey Reply", survey.ReplyMessage);

//            return Json(new { success = true });
//        }

//        // POST: Survey/Create
//        [HttpPost]
//        public IActionResult Create(Survey survey)
//        {
//            if (ModelState.IsValid)
//            {
//                survey.SubmittedAt = DateTime.Now;
//                survey.Status = "未回覆"; // 初始狀態

//                // 將問卷保存到資料庫
//                _dbContext.Survey.Add(survey);
//                _dbContext.SaveChanges();

//                return RedirectToAction(nameof(Index));
//            }

//            return View(survey);
//        }

//        // GET: Survey/Details/5
//        public IActionResult Details(int id)
//        {
//            var survey = _dbContext.Survey.FirstOrDefault(s => s.Id == id);
//            if (survey == null)
//            {
//                return NotFound();
//            }

//            return View(survey);
//        }
//    }
//}