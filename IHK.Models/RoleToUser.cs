namespace IHK.Models
{
    public class RoleToUser : BaseModel
    {

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
