namespace DatingApp.API.Helpers
{
    /// <summary>
    /// Paging defaults (PageNumber=1, PageSize=10, MinAge=18, MaxAge =99) as getters
    /// With optional setters to change these values
    /// </summary>
    public class UserParams
    {
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string Gender { get; set; }
        public string OrderBy { get; set; }
        public int UserId { get; set; }               //Filter out current user
        public int PageNumber { get; set; } = 1;    //Which page is Client on
        private int pageSize = 10;                  //max items per page

        private const int MaxPageSize = 50;

        public int PageSize     //getter/setter for more contro over backed pageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = (value > MaxPageSize) ? MaxPageSize : value;
                // if (value > 50) pageSize =50;
                // else pageSize = value;
            }
        }

    }
}