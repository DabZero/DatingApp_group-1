namespace DatingApp.API.Helpers
{
    /// <summary>
    /// Paging defaults (PageNumber=1, PageSize=10) as getters
    /// With optional setters to change these values
    /// </summary>
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        // Max users per page
        //
        private int pageSize = 10;
        public int PageSize
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