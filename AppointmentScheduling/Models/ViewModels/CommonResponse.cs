namespace AppointementScheduling.Models.ViewModels
{
    public class CommonResponse<T>
    {//use api - return response
        public int status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }
}
