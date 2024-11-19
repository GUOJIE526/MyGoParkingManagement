using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using EasyPark.Data;
using EasyPark.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPark.Areas.MySurvey.Controllers
{
    [Authorize]
    [Area("MySurvey")]
    public class SurveyController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly EasyParkContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public SurveyController(IEmailSender emailSender, EasyParkContext dbContext, IWebHostEnvironment env)
        {
            _emailSender = emailSender;
            _dbContext = dbContext;
            _env = env;
        }

        // 顯示所有問卷
        public IActionResult Index(string searchString, string filterStatus)
        {
            var surveys = _dbContext.Survey.Include(s => s.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                surveys = surveys.Where(s => s.User.Email.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(filterStatus))
            {
                surveys = filterStatus == "已回覆" ? surveys.Where(s => s.IsReplied) : surveys.Where(s => !s.IsReplied);
            }

            var surveyList = surveys.ToList();

            // Check if request is AJAX to return only the partial view for tbody
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_SurveyTbody", surveyList);
            }

            return View(surveyList);
        }

        [HttpPost]
        public async Task<IActionResult> Reply(int id, string replyMessage)
        {
            var survey = await _dbContext.Survey.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
            if (survey == null)
            {
                return Json(new { success = false, message = "問卷不存在" });
            }

            if (survey.IsReplied)
            {
                return Json(new { success = false, message = "該問卷已回覆，無法再次回覆" });
            }

            survey.ReplyMessage = replyMessage;
            survey.IsReplied = true;
            survey.RepliedAt = DateTime.Now;
            survey.Status = "已回覆";

            try
            {
                await _dbContext.SaveChangesAsync();

                // 使用 Path.Combine 獲取模板路徑
                string emailTemplatePath = Path.Combine(_env.WebRootPath, "templates", "email_template.html");

                // 確認模板文件是否存在
                if (!System.IO.File.Exists(emailTemplatePath))
                {
                    return Json(new { success = false, message = "找不到電子郵件模板文件" });
                }

                // 加載並替換 HTML 電子郵件模板中的佔位符
                string emailTemplate = await System.IO.File.ReadAllTextAsync(emailTemplatePath);
                emailTemplate = emailTemplate.Replace("{{userEmail}}", survey.User.Email)
                                             .Replace("{{userQuestion}}", survey.Question)
                                             .Replace("{{replyMessage}}", survey.ReplyMessage);

                // 發送電子郵件
                await _emailSender.SendEmailAsync(survey.User.Email, "MyGoParking 回覆通知", emailTemplate);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "回覆過程中發生錯誤：" + ex.Message });
            }
        }

        // POST: Survey/Create
        [HttpPost]
        public async Task<IActionResult> Create(Survey survey)
        {
            if (ModelState.IsValid)
            {
                survey.SubmittedAt = DateTime.Now;
                survey.Status = "未回覆"; // 初始狀態

                // 將問卷保存到資料庫
                _dbContext.Survey.Add(survey);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(survey);
        }

        // GET: Survey/Details/5
        public IActionResult Details(int id)
        {
            var survey = _dbContext.Survey.FirstOrDefault(s => s.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return View(survey);
        }

        public IActionResult GetSurveyData(string filterStatus, string searchString)
        {
            var surveys = _dbContext.Survey.Include(s => s.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                surveys = surveys.Where(s => s.User.Email.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(filterStatus))
            {
                surveys = filterStatus == "已回覆" ? surveys.Where(s => s.IsReplied) : surveys.Where(s => !s.IsReplied);
            }

            var data = surveys.Select(s => new {
                surveyId = s.Id,
                email = s.User.Email,
                submittedAt = s.SubmittedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                status = s.IsReplied ? "已回覆" : "未回覆",
                question = s.Question
            }).ToList();

            return Json(data);
        }

        [HttpGet]
        public IActionResult GetReplyMessage(int id)
        {
            var survey = _dbContext.Survey.Include(s => s.User).FirstOrDefault(s => s.Id == id);
            if (survey == null)
            {
                return Json(new { success = false, message = "問卷不存在" });
            }

            return Json(new
            {
                success = true,
                email = survey.User.Email,
                question = survey.Question,
                replyMessage = survey.ReplyMessage
            });
        }
    }
}
