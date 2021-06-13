using Newtonsoft.Json;
using StoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StoreAPI.Controllers
{
    public class FeedBackController : Controller
    {

        private StoreContext db = new StoreContext();

        [HttpGet]
        [Authorize(Roles = "admin,user")]
        public string IndexJson()
        {

            List<Feedback> feedbacks = db.Feedbacks.ToList();

            string json = JsonConvert.SerializeObject(feedbacks);

            return json;
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string AddFeedBack(FeedBackCustom model)
        {

            if (db.Feedbacks.Where(c => c.id_customer == model.id_customer).FirstOrDefault() == null && model != null)
            {

                Feedback feedback = new Feedback();

                List<Feedback> feedbacks = db.Feedbacks.ToList();

                if (feedbacks.Where(r => r.id_feedback == 1).FirstOrDefault() == null) feedback.id_feedback = 1;
                else feedback.id_feedback = feedbacks.LastOrDefault().id_feedback + 1;

                if (db.Customers.Where(c => c.id_customer == model.id_customer).ToList().FirstOrDefault() == null) return "Нет такого пользователя";
                else feedback.id_customer = model.id_customer;

                feedback.content = model.content;

                db.Feedbacks.Add(feedback);
                db.SaveChanges();

                string json = JsonConvert.SerializeObject(feedback);

                return json;
            }

            Feedback feedBackBase = db.Feedbacks.Where(c => c.id_customer == model.id_customer).FirstOrDefault();
            string jsonBase = JsonConvert.SerializeObject(feedBackBase);
            return jsonBase;

        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string GetFeedBack(FeedBackID model)
        {
            Feedback feedbackBase = db.Feedbacks.Where(c => c.id_customer == model.id_customer).FirstOrDefault();
            if (feedbackBase != null)
            {
                string jsonBase = JsonConvert.SerializeObject(feedbackBase);
                return jsonBase;
            }
            else return "Нету отзыва";
        }

        [HttpPost]
        [Route("api/[controller]")]
        [Authorize(Roles = "admin,user")]
        public string DeleteFeedBack(FeedBackID model)
        {
            if (model != null)
            {
                Feedback feedback = db.Feedbacks.Where(c => c.id_customer == model.id_customer).ToList().FirstOrDefault();

                Feedback feedbackDelete = db.Feedbacks.Find(feedback.id_feedback);

                db.Feedbacks.Remove(feedbackDelete);
                db.SaveChangesAsync();
                return "Успешно";
            }
            else return "Провал";
        }

    }

}
