namespace DoAn.ViewModels.Users
{
    public class BasicInfoUser
    {
        public Guid id { get; set; }
        public string fullName { get; set; }


        public string? address { get; set; }

        public string? CMND { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
