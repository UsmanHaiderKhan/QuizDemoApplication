using QuizDemoApplication.Models;
using QuizDemoClasses;
using QuizDemoClasses.QuizManagement;
using QuizDemoClasses.UserManagement;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QuizDemoApplication.Controllers
{
    public class StudentsController : Controller
    {
        private QuizDemoContext _db = new QuizDemoContext();
        // GET: Students
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult StudentRegistration()
        {

            ViewBag.Types = _db.Tests.Where(x => x.IsActive == true).Select(x => new { x.Id, x.Name }).ToList();

            SessionModel model = null;
            if (Session["SessionModel"] == null)
                model = new SessionModel();

            else
                model = (SessionModel)Session["SessionModel"];


            return View(model);
        }

        public ActionResult Instruction(SessionModel sessionModel)
        {
            if (sessionModel != null)
            {
                var test = _db.Tests.FirstOrDefault(c => c.IsActive == true && c.Id == sessionModel.TestId);
                if (test != null)
                {
                    ViewBag.TestName = test.Name;
                    ViewBag.TestDescription = test.Description;
                    ViewBag.QuestionCount = test.TestQuestions.Count;
                    ViewBag.TestDuration = test.DurationInMint;

                }
            }
            return View(sessionModel);

        }

        public ActionResult Register(SessionModel model)
        {
            if (model != null)
                Session["SessionModel"] = model;


            if (model == null || string.IsNullOrEmpty(model.UserName) || model.TestId < 1)
            {
                TempData["message"] = "Invalid Registration Details ,,...Please Try Again.";
                return RedirectToAction("StudentRegistration");
            }

            Student _user = _db.Students.FirstOrDefault(x => x.Name.Equals(model.UserName, StringComparison.CurrentCultureIgnoreCase)
                && ((string.IsNullOrEmpty(model.Email) && string.IsNullOrEmpty(x.Email)) || (x.Email == model.Email))
                && ((string.IsNullOrEmpty(model.PhonNo) && string.IsNullOrEmpty(x.PhonNo)) ||
                    (x.PhonNo == model.PhonNo)));

            if (_user != null)
            {
                _user = new Student()
                {
                    Name = model.UserName,
                    Email = model.Email,
                    PhonNo = model.PhonNo,
                    EntryDate = DateTime.UtcNow,
                    AccessLevel = 100
                };
                _db.Students.Add(_user);
                _db.SaveChanges();

            }

            Registration registration = _db.Registrations
                .FirstOrDefault(x => x.StudentId == _user.Id && x.TestId == model.TestId && x.TokenExpireTime > DateTime.UtcNow);

            if (registration != null)
            {
                this.Session["Token"] = registration.Token;
                this.Session["TokenExpireTime"] = registration.TokenExpireTime;

            }
            else
            {
                Test test = _db.Tests.FirstOrDefault(x => x.IsActive && x.Id == model.TestId);
                if (test != null)
                {
                    Registration newregistration = new Registration()
                    {
                        RegistrationDate = DateTime.UtcNow,
                        TestId = model.TestId,
                        Token = Guid.NewGuid(),
                        TokenExpireTime = DateTime.UtcNow.AddMinutes(test.DurationInMint)
                    };
                    // _user.Registrations.Add(newregistration);
                    _db.Registrations.Add(newregistration);
                    _db.SaveChanges();
                    this.Session["Token"] = newregistration.Token;
                    this.Session["TokenExpireTime"] = newregistration.TokenExpireTime;

                }
            }

            return View("EvalutionPage", new { token = Session["Token"] });
        }

        public ActionResult EvalutionPage(Guid token, int? qno)
        {
            if (token == null)
            {
                TempData["message"] = "Invalid Registration Details ,,...Please Try Again.";
                return RedirectToAction("StudentRegistration");
            }

            var registration = _db.Registrations.FirstOrDefault(x => x.Token.Equals(token));
            if (registration == null)
            {
                TempData["message"] = "Token is Invalid";
                return RedirectToAction("StudentRegistration");
            }

            if (registration.TokenExpireTime < DateTime.UtcNow)
            {
                TempData["message"] = "Your Test Duration Time has been expired..." + registration.TokenExpireTime.ToString();
                return RedirectToAction("StudentRegistration");
            }

            if (qno.GetValueOrDefault() < 1)

                qno = 1;
            var testQuestionId = _db.TestQuestions.Where(x => x.TestId == registration.TestId &&
                                                              x.QuestionNumber == qno).Select(x => x.Id)
                .FirstOrDefault();


            if (testQuestionId > 0)
            {
                var _model = _db.TestQuestions.Where(x => x.Id == testQuestionId).Select(x => new QuestionModel()
                {
                    QuestionType = x.Question.QuestionType,
                    QuestionNumber = x.QuestionNumber,
                    Question = x.Question.Questino1,
                    Point = x.Question.Points,
                    TestId = x.TestId,
                    TestName = x.Test.Name,
                    Options = x.Question.Choices.Where(y => y.IsActive == true).Select(y => new OptionsModel()
                    {
                        ChoiceId = y.Id,
                        Lable = y.Lable,

                    }).ToList()

                }).FirstOrDefault();
                return View(_model);
            }

            else
            {
                return View("Error");
            }
            return View();

        }

    }
}
