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
                    _user.Registrations.Add(newregistration);
                    _db.Registrations.Add(newregistration);
                    _db.SaveChanges();
                    this.Session["Token"] = newregistration.Token;
                    this.Session["TokenExpireTime"] = newregistration.TokenExpireTime;

                }
            }

            return RedirectToAction("EvalutionPage", new { token = Session["Token"] });
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





                var savedAnswer = _db.TestPapers.Where(x =>
                    x.TestQuestionId == testQuestionId && x.RegistrartionId == registration.Id &&
                    x.Choice.IsActive == true)
                    .Select(x => new { x.ChoiceId, x.Answer }).ToList();

                foreach (var saveAns in savedAnswer)
                {
                    _model.Options.FirstOrDefault(x => x.ChoiceId == saveAns.ChoiceId).Answer = saveAns.Answer;
                }




                _model.TotalQuestionInSet = _db.TestQuestions
                    .Where(x => x.Question.IsActive == true && x.TestId == registration.TestId).Count();
                ViewBag.Expire = registration.TokenExpireTime;
                return View(_model);
            }

            else
            {
                return View("Error");
            }
        }
        [HttpPost]

        public ActionResult PostAnswer(AnswerModel choices)
        {
            //verify that is Register Allow to check the question
            var registration = _db.Registrations.FirstOrDefault(x => x.Token.Equals(choices.Token));
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

            var testQuestionInfo = _db.TestQuestions
                .Where(x => x.TestId == registration.TestId && x.QuestionNumber == choices.QuestionId)
                .Select(x => new
                {
                    TQId = x.Id,
                    QT = x.Question.QuestionType,
                    QId = x.Id,
                    POINT = x.Question.Points



                }).FirstOrDefault();

            if (testQuestionInfo != null)
            {
                if (choices.ChoiceModels.Count > 1)
                {
                    var allValueofChoice =
                    (
                        from a in _db.Choices.Where(x => x.IsActive)
                        join b in choices.UserSelectedId on a.Id equals b
                        select new { a.Id, Points = (decimal)a.Points }).AsEnumerable()
                        .Select(x => new TestPaper()
                        {
                            RegistrartionId = registration.Id,
                            TestQuestionId = testQuestionInfo.QId,
                            ChoiceId = x.Id,
                            Answer = "CHECKED",
                            MarkScored = Math.Floor((testQuestionInfo.POINT / 100.00M) * x.Points),



                        }).ToList();

                    _db.TestPapers.AddRange(allValueofChoice);

                }
                else
                {
                    _db.TestPapers.Add(new TestPaper()
                    {
                        RegistrartionId = registration.Id,
                        TestQuestionId = testQuestionInfo.QId,
                        ChoiceId = choices.ChoiceModels.FirstOrDefault().ChoiceId,
                        MarkScored = 1,
                        Answer = choices.Answer
                    });
                }

                _db.SaveChanges();
            }

            var nextQuestionNo = 1;
            if (choices.Direction.Equals("forward", StringComparison.CurrentCultureIgnoreCase))
            {
                nextQuestionNo = _db.TestQuestions.Where(x => x.TestId == choices.TestId
                                                              && x.QuestionNumber > choices.QuestionId)
                    .OrderBy(x => x.QuestionNumber).Take(1).Select(x => x.QuestionNumber).FirstOrDefault();
            }
            else
            {

                nextQuestionNo = _db.TestQuestions.Where(x => x.TestId == choices.TestId
                                                              && x.QuestionNumber > choices.QuestionId)
                    .OrderByDescending(x => x.QuestionNumber).Take(1).Select(x => x.QuestionNumber).FirstOrDefault();
            }

            if (nextQuestionNo < 1)
            {
                nextQuestionNo = 1;

            }



            return RedirectToAction("EvalutionPage", new
            {
                token = Session["TOKEN"],
                qno = nextQuestionNo
            });
        }
    }
}
