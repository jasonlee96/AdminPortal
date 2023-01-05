namespace AdminPortal.Api.Domains
{
    public class GetJwtTokenFilter
    {
        public string Token { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int UserId { get; set; }
    }
}
