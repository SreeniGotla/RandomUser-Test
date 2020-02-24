#region

using System;


#endregion

namespace DemoRandomUser.Model
{
    public interface IUpdatable
    {
        string CreatedBy { get; set; }
        DateTime? CreatedOnUtc { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedOnUtc { get; set; }
    }
}