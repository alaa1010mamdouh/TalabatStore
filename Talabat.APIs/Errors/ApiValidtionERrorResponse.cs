namespace Talabat.APIs.Errors
{
    public class ApiValidtionERrorResponse:ApiResponse
    {
        public ApiValidtionERrorResponse() : base(400)
        {
            Errors=new List<string>();
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
