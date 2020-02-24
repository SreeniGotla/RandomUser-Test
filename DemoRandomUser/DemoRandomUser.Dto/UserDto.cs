using System;

namespace DemoRandomUser.Dto
{
    public class UserDto 
    {
        public int UserId { get; set; }
        public string UserTitle { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public DateTime DOB { get; set; }
        public int PhoneNumber { get; set; }
        public string Image { get; set; }

        public string DeletedBy { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
        public int ObjectId => UserId;
        public string CreatedBy { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}
