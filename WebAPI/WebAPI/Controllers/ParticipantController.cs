using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ParticipantController : ApiController
    {
        [HttpPost]
        [Route("api/InsertParticipant")]
        public Participant Insert(Participant model)
        {
           List<Participant> res = GetParticipants(model);
            if (res.Count == 0)
            {
                using (DBModel db = new DBModel())
                {
                    db.Participants.Add(model);
                    db.SaveChanges();

                    return model;
                }
            }
            else
            {
                return model;
            }
        }

        [HttpPost]
        [Route("api/UpdateOutput")]
        public void UpdateOutput(Participant model)
        {
            using (DBModel db = new DBModel())
            {
                db.Entry(model).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }


        public List<Participant> GetParticipants(Participant model)
        {
            using (DBModel db = new DBModel())
            {
                {
                    var result = db.Participants
                        .Where(x => model.Email.Equals(x.Email))
                       .Select(x => new
                       {
                           x.ParticipantID,
                           x.Email,
                           x.Name,
                           x.Score,
                           x.TimeSpent
                       })
                       .OrderBy(y => Guid.NewGuid())
                       .Take(10)
                       .ToArray();
                    var updated = result.AsEnumerable()
                        .Select(x => new
                        {
                            x.ParticipantID,
                            x.Email,
                            x.Name,
                            x.Score,
                            x.TimeSpent
                        }).ToList();
                    List<Participant> participant = new List<Participant>();
                    foreach(var i in updated)
                    {
                        model.Name = i.Name;
                        model.Email = i.Email;
                        model.Score = i.Score;
                        model.TimeSpent = i.TimeSpent;
                        participant.Add(model);
                    }
                    return participant;
                        

                }

            }
        }
    }
}