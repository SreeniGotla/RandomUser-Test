#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace DemoRandomUser.Model
{
    public class AbstractModel
    {
        [MaxLength(64)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [MaxLength(64)]
        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }
}