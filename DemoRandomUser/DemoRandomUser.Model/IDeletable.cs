#region

using System;

#endregion

namespace DemoRandomUser.Model
{
    public interface IDeletable
    {
        string DeletedBy { get; set; }
        DateTime? DeletedOnUtc { get; set; }
    }
}