namespace WebApplicationAPI.Interfaces.Services
{
    public interface IStackOverflowApiService
    {
        public Task GetTagsAsync(int pagesToFetch);
    }
}
