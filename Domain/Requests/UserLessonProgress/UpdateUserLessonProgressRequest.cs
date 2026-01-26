using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests.UserLessonProgress
{
    public class UpdateUserLessonProgressRequest
    {
        public Guid LessonId { get; set; }
        public int? LastWatchedSecond { get; set; }
        public int? CompletionPercent { get; set; }
    }
}
