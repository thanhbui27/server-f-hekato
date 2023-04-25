namespace DoAn.Helpers.ApiResponse
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult(T resultObj)
        {
            IsSuccessed = true;
            Data = resultObj;
        }

        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }
    }
}
