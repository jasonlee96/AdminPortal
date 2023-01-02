namespace CommonService.Options
{
    public class JwtOption
    {
        public string SecretKey { get; set; }
        public int? KeyDuration { get; set; }
    }
}
