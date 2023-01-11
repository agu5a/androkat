namespace androkat.web.Endpoints.ContentDetailsModelEndpoints;

public class ContentDetailsModelResponse
{
    public ContentDetailsModelResponse(bool result)
    {
        Result = result;
    }

    public bool Result { get; init; }
}